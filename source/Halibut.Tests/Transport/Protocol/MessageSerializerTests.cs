﻿using System.Buffers.Text;
using System.IO;
using System.Text;
using FluentAssertions;
using Halibut.Exceptions;
using Halibut.Transport;
using Halibut.Transport.Protocol;
using NUnit.Framework;

namespace Halibut.Tests.Transport.Protocol
{
    public class MessageSerializerTests
    {
        [Test]
        public void SendReceiveMessageShouldRoundTrip()
        {
            var sut = MessageSerializerBuilder.Build();
            using (var stream = new MemoryStream())
            {
                sut.WriteMessage(stream, "Test");
                stream.Position = 0;
                var result = sut.ReadMessage<string>(stream);
                Assert.AreEqual("Test", result);
            }
        }

        [Test]
        public void BackwardsCompatability_ExtraParametersInServerErrorAreIgnored()
        {
            var sut = MessageSerializerBuilder.Build();
            // This is bson of a RequestMessage which contains a hacked ServerError which has an extra field which no version of Halibut will ever have.
            // The test expects that the serializer will ignore the extra field. 
            // This is to show that as extra fields can be added to the ServerError and they will be ignored.
            var base64Bson = "nY6xTgNBDESHDTR8xRakQ1dRICSaRIfSBCHIDyxZk1tpsz6tfQE+mP/AgQVRU7gYe96Mn06A2ZpEwo7Qm3AX+j4SrgCsQk7Pk3abGoqMXLV7qKy85dw9ki2KUCMvffPCpYgzYyVKxIxq5YoP027fOk5NvDDDRdKQsuD2T1P/tqVRkyV3a9KB4z3rHU8lNsMyJyr667rxX0nt2B/LNsfnr/8fCbfIYfgZzC3JAC+8NziVnR++Mf+acvZ0oGqbAwHnlWTKav5P";
            using (var stream = new MemoryStream(System.Convert.FromBase64String(base64Bson)))
            {
                var result = sut.ReadMessage<ResponseMessage>(stream);
                result.Error.Should().NotBeNull();
                result.Error.Message = "foo";
                result.Error.HalibutErrorType = "MethodNotFoundHalibutClientException";
            }
        }

        #if !NETFRAMEWORK
        [Test]
        public void SendReceiveMessageRewindableShouldRoundTrip()
        {
            using (var stream = new MemoryStream())
            using (var rewindableStream = new RewindableBufferStream(stream))
            {
                var sut = MessageSerializerBuilder.Build();
                sut.WriteMessage(stream, "Test");
                stream.Position = 0;
                Assert.AreEqual("Test", sut.ReadMessage<string>(rewindableStream));
            }
        }

        [Test]
        public void ReadMessageRewindableShouldNotConsumeTrailingData()
        {
            const string trailingData = "SomeOtherData";

            var sut = MessageSerializerBuilder.Build();
            using (var ms = new MemoryStream())
            using (var stream = new RewindableBufferStream(ms))
            {
                sut.WriteMessage(stream, "Test");
                var trailingBytes = Encoding.UTF8.GetBytes(trailingData);
                stream.Write(trailingBytes, 0, trailingBytes.Length);
                ms.Position = 0;

                using (var reader = new StreamReader(stream, new UTF8Encoding(false)))
                {
                    _ = sut.ReadMessage<string>(stream);
                    var trailingResult = reader.ReadToEnd();
                    Assert.AreEqual(trailingData, trailingResult);
                }
            }
        }
        #endif

        static class MessageSerializerBuilder
        {
            public static MessageSerializer Build() => new MessageSerializer();
        }
    }
}
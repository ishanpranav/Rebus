// Ishan Pranav's REBUS: RpcClient.cs
// Copyright (c) 2021-2022 Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using StreamJsonRpc;

namespace Rebus.Client
{
    public class RpcClient : IAsyncDisposable
    {
        private TcpClient? _tcpClient = new TcpClient();
        private NetworkStream? _networkStream;
        private MessagePackFormatter? _messageFormatter = new MessagePackFormatter();
        private LengthHeaderMessageHandler? _messageHandler;
        private JsonRpc? _jsonRpc;
        private IDisposable? _service;

        public async Task<T> CreateAsync<T>(IPAddress ipAddress, int port) where T : class, IDisposable
        {
            if (_tcpClient == null || _messageFormatter == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                await _tcpClient.ConnectAsync(ipAddress, port);

                _networkStream = _tcpClient.GetStream();
                _messageHandler = new LengthHeaderMessageHandler(_networkStream, _networkStream, _messageFormatter);
                _jsonRpc = new JsonRpc(_messageHandler);

                T result = _jsonRpc.Attach<T>();

                _service = result;

                return result;
            }
        }

        public void Start()
        {
            _jsonRpc?.StartListening();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(continueOnCapturedContext: false);

            GC.SuppressFinalize(this);
        }

        [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "DisposeAsyncCore")]
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_networkStream != null)
            {
                await _networkStream
                    .DisposeAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

                _networkStream = null;
            }

            if (_messageHandler != null)
            {
                await _messageHandler
                    .DisposeAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);

                _messageHandler = null;
            }

            _tcpClient?.Dispose();
            _tcpClient = null;

            _messageFormatter?.Dispose();
            _messageFormatter = null;

            _jsonRpc?.Dispose();
            _jsonRpc = null;

            _service?.Dispose();
            _service = null;
        }
    }
}

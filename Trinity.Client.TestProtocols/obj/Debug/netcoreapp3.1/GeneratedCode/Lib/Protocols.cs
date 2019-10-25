#pragma warning disable 162,168,649,660,661,1522
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trinity;
using Trinity.TSL;
using Trinity.Core.Lib;
using Trinity.Network;
using Trinity.Network.Messaging;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Trinity.Storage;
namespace Trinity.Client.TestProtocols
{
    
    public abstract partial class TripleServerBase : CommunicationModule
    {
        protected override void RegisterMessageHandler()
        {
            
            {
                
                MessageRegistry.RegisterMessageHandler((ushort)(this.AsynReqRspIdOffset + (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.StreamTriplesAsync), _StreamTriplesAsyncHandler);
                MessageRegistry.RegisterMessageHandler((ushort)(this.AsynReqRspIdOffset + (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.StreamTriplesAsync__Response), _StreamTriplesAsync_ResponseHandler);
                
            }
            
            {
                
                MessageRegistry.RegisterMessageHandler((ushort)(this.AsynReqRspIdOffset + (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.PostTriplesToServer), _PostTriplesToServerHandler);
                MessageRegistry.RegisterMessageHandler((ushort)(this.AsynReqRspIdOffset + (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.PostTriplesToServer__Response), _PostTriplesToServer_ResponseHandler);
                
            }
            
        }
        
        private unsafe void _StreamTriplesAsyncHandler(AsynReqRspArgs args)
        {
            using (var rsp = new ErrorCodeResponseWriter(asyncRspHeaderLength: TrinityProtocol.AsyncWithRspAdditionalHeaderLength))
            {
                Exception exception = null;
                var req = new TripleStreamReader(args.Buffer, args.Offset + TrinityProtocol.AsyncWithRspAdditionalHeaderLength);
                try { StreamTriplesAsyncHandler(req, rsp); }
                catch (Exception ex) { exception = ex; }
                int token = *(int*)(args.Buffer + args.Offset);
                int from = *(int*)(args.Buffer + args.Offset + sizeof(int));
                _StreamTriplesAsync_CheckError(exception, token, from);
                *(int*)(rsp.buffer) = TrinityProtocol.TrinityMsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength + rsp.Length;
                *(TrinityMessageType*)(rsp.buffer + TrinityProtocol.MsgTypeOffset) = TrinityMessageType.ASYNC_WITH_RSP;
                *(ushort*)(rsp.buffer + TrinityProtocol.MsgIdOffset) = (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.StreamTriplesAsync__Response;
                *(int*)(rsp.m_ptr - TrinityProtocol.AsyncWithRspAdditionalHeaderLength) = token;
                *(int*)(rsp.m_ptr - TrinityProtocol.AsyncWithRspAdditionalHeaderLength + sizeof(int)) = 0;
                
                this.SendMessage(m_memorycloud[from], rsp.buffer, rsp.Length + TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength);
                
            }
        }
        public abstract void StreamTriplesAsyncHandler(TripleStreamReader request, ErrorCodeResponseWriter response);
        
        #region AsyncWithRsp
        internal static int s_StreamTriplesAsync_token_counter = 0;
        internal static ConcurrentDictionary<int, TaskCompletionSource<ErrorCodeResponseReader>> s_StreamTriplesAsync_token_sources = new ConcurrentDictionary<int, TaskCompletionSource<ErrorCodeResponseReader>>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void _StreamTriplesAsync_CheckError(Exception exception, int token, int from)
        {
            if (exception == null) return;
            byte[] rsp = new byte[TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength];
            fixed (byte* p = rsp)
            {
                *(int*)(p) = TrinityProtocol.TrinityMsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
                *(TrinityMessageType*)(p + TrinityProtocol.MsgTypeOffset) = TrinityMessageType.ASYNC_WITH_RSP;
                *(ushort*)(p + TrinityProtocol.MsgIdOffset) = (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.StreamTriplesAsync__Response;
                *(int*)(p + TrinityProtocol.MsgHeader) = token;
                *(int*)(p + TrinityProtocol.MsgHeader + sizeof(int)) = -1;
                
                this.SendMessage(m_memorycloud[from], p, rsp.Length);
                
            }
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
        internal unsafe void _StreamTriplesAsync_ResponseHandler(AsynReqRspArgs args)
        {
            byte* buffer = args.Buffer + args.Offset;
            int size = args.Size - TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
            if (size < 0)
            {
                throw new ArgumentException("Async task completion handler encountered negative message size.");
            }
            int token = *(int*)buffer;
            int error = *(int*)(buffer + sizeof(int));
            if (!s_StreamTriplesAsync_token_sources.TryRemove(token, out var src))
            {
                throw new ArgumentException("Async task completion token not found while processing a AsyncWithResponse message.");
            }
            if (error != 0)
            {
                src.SetException(new Exception("AsyncWithResponse remote handler failed."));
                return;
            }
            byte* buffer_clone = (byte*)Memory.malloc((ulong)(args.Size));
            Memory.Copy(buffer, buffer_clone, args.Size);
            var reader = new ErrorCodeResponseReader(buffer_clone, TrinityProtocol.AsyncWithRspAdditionalHeaderLength);
            try { src.SetResult(reader); }
            catch { Memory.free(buffer_clone); throw; }
        }
        #endregion
        
        private unsafe void _PostTriplesToServerHandler(AsynReqRspArgs args)
        {
            using (var rsp = new ErrorCodeResponseWriter(asyncRspHeaderLength: TrinityProtocol.AsyncWithRspAdditionalHeaderLength))
            {
                Exception exception = null;
                var req = new TripleStreamReader(args.Buffer, args.Offset + TrinityProtocol.AsyncWithRspAdditionalHeaderLength);
                try { PostTriplesToServerHandler(req, rsp); }
                catch (Exception ex) { exception = ex; }
                int token = *(int*)(args.Buffer + args.Offset);
                int from = *(int*)(args.Buffer + args.Offset + sizeof(int));
                _PostTriplesToServer_CheckError(exception, token, from);
                *(int*)(rsp.buffer) = TrinityProtocol.TrinityMsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength + rsp.Length;
                *(TrinityMessageType*)(rsp.buffer + TrinityProtocol.MsgTypeOffset) = TrinityMessageType.ASYNC_WITH_RSP;
                *(ushort*)(rsp.buffer + TrinityProtocol.MsgIdOffset) = (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.PostTriplesToServer__Response;
                *(int*)(rsp.m_ptr - TrinityProtocol.AsyncWithRspAdditionalHeaderLength) = token;
                *(int*)(rsp.m_ptr - TrinityProtocol.AsyncWithRspAdditionalHeaderLength + sizeof(int)) = 0;
                
                this.SendMessage(m_memorycloud[from], rsp.buffer, rsp.Length + TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength);
                
            }
        }
        public abstract void PostTriplesToServerHandler(TripleStreamReader request, ErrorCodeResponseWriter response);
        
        #region AsyncWithRsp
        internal static int s_PostTriplesToServer_token_counter = 0;
        internal static ConcurrentDictionary<int, TaskCompletionSource<ErrorCodeResponseReader>> s_PostTriplesToServer_token_sources = new ConcurrentDictionary<int, TaskCompletionSource<ErrorCodeResponseReader>>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private unsafe void _PostTriplesToServer_CheckError(Exception exception, int token, int from)
        {
            if (exception == null) return;
            byte[] rsp = new byte[TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength];
            fixed (byte* p = rsp)
            {
                *(int*)(p) = TrinityProtocol.TrinityMsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
                *(TrinityMessageType*)(p + TrinityProtocol.MsgTypeOffset) = TrinityMessageType.ASYNC_WITH_RSP;
                *(ushort*)(p + TrinityProtocol.MsgIdOffset) = (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.PostTriplesToServer__Response;
                *(int*)(p + TrinityProtocol.MsgHeader) = token;
                *(int*)(p + TrinityProtocol.MsgHeader + sizeof(int)) = -1;
                
                this.SendMessage(m_memorycloud[from], p, rsp.Length);
                
            }
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
        internal unsafe void _PostTriplesToServer_ResponseHandler(AsynReqRspArgs args)
        {
            byte* buffer = args.Buffer + args.Offset;
            int size = args.Size - TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
            if (size < 0)
            {
                throw new ArgumentException("Async task completion handler encountered negative message size.");
            }
            int token = *(int*)buffer;
            int error = *(int*)(buffer + sizeof(int));
            if (!s_PostTriplesToServer_token_sources.TryRemove(token, out var src))
            {
                throw new ArgumentException("Async task completion token not found while processing a AsyncWithResponse message.");
            }
            if (error != 0)
            {
                src.SetException(new Exception("AsyncWithResponse remote handler failed."));
                return;
            }
            byte* buffer_clone = (byte*)Memory.malloc((ulong)(args.Size));
            Memory.Copy(buffer, buffer_clone, args.Size);
            var reader = new ErrorCodeResponseReader(buffer_clone, TrinityProtocol.AsyncWithRspAdditionalHeaderLength);
            try { src.SetResult(reader); }
            catch { Memory.free(buffer_clone); throw; }
        }
        #endregion
        
    }
    
    namespace TripleServer
    {
        public static class MessagePassingExtension
        {
            
        #region prototype definition template variables
        
        #endregion
        
        public unsafe static Task<ErrorCodeResponseReader> StreamTriplesAsync(this Trinity.Storage.IMessagePassingEndpoint storage, TripleStreamWriter msg)
        {
            byte** bufferPtrs = stackalloc byte*[2];
            int*   size       = stackalloc int[2];
            byte*  bufferPtr  = stackalloc byte[TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength];
            bufferPtrs[0]     = bufferPtr;
            bufferPtrs[1]     = msg.buffer + TrinityProtocol.MsgHeader;
            size[0]           = TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
            size[1]           = msg.Length;
            int token = Interlocked.Increment(ref TripleServerBase.s_StreamTriplesAsync_token_counter);
            var task_source = new TaskCompletionSource<ErrorCodeResponseReader>();
            TripleServerBase.s_StreamTriplesAsync_token_sources[token] = task_source;
            *(int*)(bufferPtr) = TrinityProtocol.TrinityMsgHeader + msg.Length + TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
            *(TrinityMessageType*)(bufferPtr + TrinityProtocol.MsgTypeOffset) = TrinityMessageType.ASYNC_WITH_RSP ;
            *(ushort*)(bufferPtr + TrinityProtocol.MsgIdOffset) = (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.StreamTriplesAsync;
            *(int*)(bufferPtr + TrinityProtocol.MsgHeader) = token;
            *(int*)(bufferPtr + TrinityProtocol.MsgHeader + sizeof(int)) = Global.CloudStorage.MyInstanceId;
            storage.SendMessage<TripleServerBase>(bufferPtrs, size, 2);
            return task_source.Task;
        }
        
        #region prototype definition template variables
        
        #endregion
        
        public unsafe static Task<ErrorCodeResponseReader> PostTriplesToServer(this Trinity.Storage.IMessagePassingEndpoint storage, TripleStreamWriter msg)
        {
            byte** bufferPtrs = stackalloc byte*[2];
            int*   size       = stackalloc int[2];
            byte*  bufferPtr  = stackalloc byte[TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength];
            bufferPtrs[0]     = bufferPtr;
            bufferPtrs[1]     = msg.buffer + TrinityProtocol.MsgHeader;
            size[0]           = TrinityProtocol.MsgHeader + TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
            size[1]           = msg.Length;
            int token = Interlocked.Increment(ref TripleServerBase.s_PostTriplesToServer_token_counter);
            var task_source = new TaskCompletionSource<ErrorCodeResponseReader>();
            TripleServerBase.s_PostTriplesToServer_token_sources[token] = task_source;
            *(int*)(bufferPtr) = TrinityProtocol.TrinityMsgHeader + msg.Length + TrinityProtocol.AsyncWithRspAdditionalHeaderLength;
            *(TrinityMessageType*)(bufferPtr + TrinityProtocol.MsgTypeOffset) = TrinityMessageType.ASYNC_WITH_RSP ;
            *(ushort*)(bufferPtr + TrinityProtocol.MsgIdOffset) = (ushort)global::Trinity.Client.TestProtocols.TSL.CommunicationModule.TripleServer.AsynReqRspMessageType.PostTriplesToServer;
            *(int*)(bufferPtr + TrinityProtocol.MsgHeader) = token;
            *(int*)(bufferPtr + TrinityProtocol.MsgHeader + sizeof(int)) = Global.CloudStorage.MyInstanceId;
            storage.SendMessage<TripleServerBase>(bufferPtrs, size, 2);
            return task_source.Task;
        }
        
        }
    }
    
    #region Legacy
    public static class LegacyMessagePassingExtension
    {
        
    }
    
    public abstract partial class TripleServerBase : CommunicationModule
    {
        
        #region prototype definition template variables
        
        #endregion
        
        public unsafe Task<ErrorCodeResponseReader> StreamTriplesAsync( int partitionId, TripleStreamWriter msg)
        {
            return TripleServer.MessagePassingExtension.StreamTriplesAsync(m_memorycloud[partitionId], msg);
        }
        
        #region prototype definition template variables
        
        #endregion
        
        public unsafe Task<ErrorCodeResponseReader> PostTriplesToServer( int partitionId, TripleStreamWriter msg)
        {
            return TripleServer.MessagePassingExtension.PostTriplesToServer(m_memorycloud[partitionId], msg);
        }
        
    }
    
    #endregion
}

#pragma warning restore 162,168,649,660,661,1522

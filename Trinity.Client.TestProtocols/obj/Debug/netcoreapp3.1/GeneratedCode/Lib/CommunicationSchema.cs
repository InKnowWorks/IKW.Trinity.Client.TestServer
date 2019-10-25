#pragma warning disable 162,168,649,660,661,1522

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trinity;
using Trinity.Network;
using Trinity.Network.Http;
using Trinity.Network.Messaging;
using Trinity.TSL;
using Trinity.TSL.Lib;
namespace Trinity.Client.TestProtocols
{
    internal class ProtocolDescriptor : IProtocolDescriptor
    {
        public string Name
        {
            get;
            set;
        }
        public string RequestSignature
        {
            get;
            set;
        }
        public string ResponseSignature
        {
            get;
            set;
        }
        public TrinityMessageType Type
        {
            get;
            set;
        }
    }
    #region Server
    
    #endregion
    #region Proxy
    
    #endregion
    #region Module
    
    public class TripleServerCommunicationSchema : ICommunicationSchema
    {
        IEnumerable<IProtocolDescriptor> ICommunicationSchema.SynReqProtocolDescriptors
        {
            get
            {
                string request_sig;
                
                yield break;
            }
        }
        IEnumerable<IProtocolDescriptor> ICommunicationSchema.SynReqRspProtocolDescriptors
        {
            get
            {
                string request_sig, response_sig;
                
                yield break;
            }
        }
        IEnumerable<IProtocolDescriptor> ICommunicationSchema.AsynReqProtocolDescriptors
        {
            get
            {
                string request_sig;
                
                yield break;
            }
        }
        IEnumerable<IProtocolDescriptor> ICommunicationSchema.AsynReqRspProtocolDescriptors
        {
            get
            {
                string request_sig;
                string response_sig;
                
                {
                    
                    request_sig = "{List<{string|string|string|string}>}";
                    
                    response_sig = "{int}";
                    yield return new ProtocolDescriptor()
                    {
                        Name = "StreamTriplesAsync",
                        RequestSignature = request_sig,
                        ResponseSignature = response_sig,
                        Type = Trinity.Network.Messaging.TrinityMessageType.ASYNC_WITH_RSP
                    };
                }
                
                {
                    
                    request_sig = "{List<{string|string|string|string}>}";
                    
                    response_sig = "{int}";
                    yield return new ProtocolDescriptor()
                    {
                        Name = "PostTriplesToServer",
                        RequestSignature = request_sig,
                        ResponseSignature = response_sig,
                        Type = Trinity.Network.Messaging.TrinityMessageType.ASYNC_WITH_RSP
                    };
                }
                
                yield break;
            }
        }
        string ICommunicationSchema.Name
        {
            get { return "TripleServer"; }
        }
        IEnumerable<string> ICommunicationSchema.HttpEndpointNames
        {
            get
            {
                
                yield break;
            }
        }
    }
    [CommunicationSchema(typeof(TripleServerCommunicationSchema))]
    public abstract partial class TripleServerBase : CommunicationModule { }
    namespace TSL.CommunicationModule.TripleServer
    {
        /// <summary>
        /// Specifies the type of a synchronous request (without response, that is, response type is void) message.
        /// </summary>
        public enum SynReqMessageType : ushort
        {
            
        }
        /// <summary>
        /// Specifies the type of a synchronous request (with response) message.
        /// </summary>
        public enum SynReqRspMessageType : ushort
        {
            
        }
        /// <summary>
        /// Specifies the type of an asynchronous request (without response) message.
        /// </summary>
        public enum AsynReqMessageType : ushort
        {
            
        }
        /// <summary>
        /// Specifies the type of an asynchronous request (with response) message.
        /// </summary>
        public enum AsynReqRspMessageType : ushort
        {
            StreamTriplesAsync,
            StreamTriplesAsync__Response,
            PostTriplesToServer,
            PostTriplesToServer__Response,
            
        }
    }
    
    #endregion
    
}

#pragma warning restore 162,168,649,660,661,1522

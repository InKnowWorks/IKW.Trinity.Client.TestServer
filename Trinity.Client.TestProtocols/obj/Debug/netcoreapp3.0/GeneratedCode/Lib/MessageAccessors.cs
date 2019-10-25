#pragma warning disable 162,168,649,660,661,1522
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity.Core.Lib;
using Trinity.Network.Messaging;
using Trinity.Storage;
using Trinity.TSL;
using Trinity.TSL.Lib;
namespace Trinity.Client.TestProtocols
{
    
    /// <summary>
    /// Represents a read-only accessor on the message of type TripleStream defined in the TSL protocols.
    /// The message readers will be instantiated by the system and passed to user's logic.
    /// After finished accessing the message. It is the user's responsibility to call Dispose()
    /// on the reader object. Recommend wrapping the reader with a <c>using Statement block</c>.
    /// <seealso ref="https://msdn.microsoft.com/en-us/library/yh598w02.aspx"/>
    /// </summary>
    public unsafe sealed class TripleStreamReader : TripleStream_Accessor, IDisposable
    {
        byte * buffer;
        internal TripleStreamReader(byte* buf, int offset)
            : base(buf + offset
                  
                  , ReaderResizeFunc
                   )
        {
            buffer = buf;
        }
        
        /** 
         * TripleStreamReader is not resizable because it may be attached
         * to a buffer passed in from the network layer and we don't know how
         * to resize it.
         */
        static byte* ReaderResizeFunc(byte* ptr, int offset, int delta)
        {
            throw new InvalidOperationException("TripleStreamReader is not resizable");
        }
        
        /// <summary>
        /// Dispose the message reader and release the memory resource.
        /// It is the user's responsibility to call this method after finished accessing the message.
        /// </summary>
        public void Dispose()
        {
            Memory.free(buffer);
            buffer = null;
        }
    }
    /// <summary>
    /// Represents a writer accessor on the message of type TripleStream defined in the TSL protocols.
    /// The message writers should be instantiated by the user's logic and passed to the system to send it out.
    /// After finished accessing the message. It is the user's responsibility to call Dispose()
    /// on the writer object. Recommend wrapping the reader with a <c>using Statement block</c>.
    /// </summary>
    /// <seealso ref="https://msdn.microsoft.com/en-us/library/yh598w02.aspx"/>
    /// <remarks>Calling <c>Dispose()</c> does not send the message out.</remarks>
    public unsafe sealed class TripleStreamWriter : TripleStream_Accessor, IDisposable
    {
        internal byte* buffer = null;
        internal int BufferLength;
        internal int Length; 
        public unsafe TripleStreamWriter( List<Triple> triples = default(List<Triple>) )
            : base(null
                  
                  , null
                   )
        {
            int preservedHeaderLength = TrinityProtocol.MsgHeader;
            
            byte* targetPtr;
            
            targetPtr = (byte*)preservedHeaderLength;
            
            {

{

    targetPtr += sizeof(int);
    if(triples!= null)
    {
        for(int iterator_2 = 0;iterator_2<triples.Count;++iterator_2)
        {

            {

        if(triples[iterator_2].Subject!= null)
        {
            int strlen_4 = triples[iterator_2].Subject.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Predicate!= null)
        {
            int strlen_4 = triples[iterator_2].Predicate.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Object!= null)
        {
            int strlen_4 = triples[iterator_2].Object.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Namespace!= null)
        {
            int strlen_4 = triples[iterator_2].Namespace.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
        }
    }

}

            }
            byte* tmpcellptr = (byte*)Memory.malloc((ulong)targetPtr);
            {
                BufferLength     = (int)targetPtr;
                Memory.memset(tmpcellptr, 0, (ulong)targetPtr);
                targetPtr = tmpcellptr;
                tmpcellptr += preservedHeaderLength;
                targetPtr  += preservedHeaderLength;
                
            {

{
byte *storedPtr_2 = targetPtr;

    targetPtr += sizeof(int);
    if(triples!= null)
    {
        for(int iterator_2 = 0;iterator_2<triples.Count;++iterator_2)
        {

            {

        if(triples[iterator_2].Subject!= null)
        {
            int strlen_4 = triples[iterator_2].Subject.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Subject)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Predicate!= null)
        {
            int strlen_4 = triples[iterator_2].Predicate.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Predicate)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Object!= null)
        {
            int strlen_4 = triples[iterator_2].Object.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Object)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Namespace!= null)
        {
            int strlen_4 = triples[iterator_2].Namespace.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Namespace)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
        }
    }
*(int*)storedPtr_2 = (int)(targetPtr - storedPtr_2 - 4);

}

            }
            }
            
            buffer = tmpcellptr - preservedHeaderLength;
            this.m_ptr = buffer + preservedHeaderLength;
            Length = BufferLength - preservedHeaderLength;
            
            this.ResizeFunction = WriterResizeFunction;
            
        }
        internal unsafe TripleStreamWriter(int asyncRspHeaderLength,  List<Triple> triples = default(List<Triple>) )
            : base(null
                  
                  , null
                   )
        {
            int preservedHeaderLength = TrinityProtocol.MsgHeader + asyncRspHeaderLength;
            
            byte* targetPtr;
            
            targetPtr = (byte*)preservedHeaderLength;
            
            {

{

    targetPtr += sizeof(int);
    if(triples!= null)
    {
        for(int iterator_2 = 0;iterator_2<triples.Count;++iterator_2)
        {

            {

        if(triples[iterator_2].Subject!= null)
        {
            int strlen_4 = triples[iterator_2].Subject.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Predicate!= null)
        {
            int strlen_4 = triples[iterator_2].Predicate.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Object!= null)
        {
            int strlen_4 = triples[iterator_2].Object.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Namespace!= null)
        {
            int strlen_4 = triples[iterator_2].Namespace.Length * 2;
            targetPtr += strlen_4+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
        }
    }

}

            }
            byte* tmpcellptr = (byte*)Memory.malloc((ulong)targetPtr);
            {
                BufferLength     = (int)targetPtr;
                Memory.memset(tmpcellptr, 0, (ulong)targetPtr);
                targetPtr = tmpcellptr;
                tmpcellptr += preservedHeaderLength;
                targetPtr  += preservedHeaderLength;
                
            {

{
byte *storedPtr_2 = targetPtr;

    targetPtr += sizeof(int);
    if(triples!= null)
    {
        for(int iterator_2 = 0;iterator_2<triples.Count;++iterator_2)
        {

            {

        if(triples[iterator_2].Subject!= null)
        {
            int strlen_4 = triples[iterator_2].Subject.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Subject)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Predicate!= null)
        {
            int strlen_4 = triples[iterator_2].Predicate.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Predicate)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Object!= null)
        {
            int strlen_4 = triples[iterator_2].Object.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Object)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(triples[iterator_2].Namespace!= null)
        {
            int strlen_4 = triples[iterator_2].Namespace.Length * 2;
            *(int*)targetPtr = strlen_4;
            targetPtr += sizeof(int);
            fixed(char* pstr_4 = triples[iterator_2].Namespace)
            {
                Memory.Copy(pstr_4, targetPtr, strlen_4);
                targetPtr += strlen_4;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
        }
    }
*(int*)storedPtr_2 = (int)(targetPtr - storedPtr_2 - 4);

}

            }
            }
            
            buffer = tmpcellptr - preservedHeaderLength;
            this.m_ptr = buffer + preservedHeaderLength;
            Length = BufferLength - preservedHeaderLength;
            
            this.ResizeFunction = WriterResizeFunction;
            
        }
        
        private byte* WriterResizeFunction(byte* ptr, int ptr_offset, int delta)
        {
            int curlen = Length;
            int tgtlen = curlen + delta;
            if (delta >= 0)
            {
                byte* currentBufferPtr = buffer;
                int required_length = (int)(tgtlen + (this.m_ptr - currentBufferPtr));
                if(required_length < curlen) throw new AccessorResizeException("Accessor size overflow.");
                if (required_length <= BufferLength)
                {
                    Memory.memmove(
                        ptr + ptr_offset + delta,
                        ptr + ptr_offset,
                        (ulong)(curlen - (ptr + ptr_offset - this.m_ptr)));
                    Length = tgtlen;
                    return ptr;
                }
                else
                {
                    while (BufferLength < required_length)
                    {
                        if (int.MaxValue - BufferLength >= (BufferLength>>1)) BufferLength += (BufferLength >> 1);
                        else if (int.MaxValue - BufferLength >= (1 << 20)) BufferLength += (1 << 20);
                        else BufferLength = int.MaxValue;
                    }
                    byte* tmpBuffer = (byte*)Memory.malloc((ulong)BufferLength);
                    Memory.memcpy(
                        tmpBuffer,
                        currentBufferPtr,
                        (ulong)(ptr + ptr_offset - currentBufferPtr));
                    byte* newCellPtr = tmpBuffer + (this.m_ptr - currentBufferPtr);
                    Memory.memcpy(
                        newCellPtr + (ptr_offset + delta),
                        ptr + ptr_offset,
                        (ulong)(curlen - (ptr + ptr_offset - this.m_ptr)));
                    Length = tgtlen;
                    this.m_ptr = newCellPtr;
                    Memory.free(buffer);
                    buffer = tmpBuffer;
                    return tmpBuffer + (ptr - currentBufferPtr);
                }
            }
            else
            {
                if (curlen + delta < 0) throw new AccessorResizeException("Accessor target size underflow.");
                Memory.memmove(
                    ptr + ptr_offset,
                    ptr + ptr_offset - delta,
                    (ulong)(Length - (ptr + ptr_offset - delta - this.m_ptr)));
                Length = tgtlen;
                return ptr;
            }
        }
        
        /// <summary>
        /// Dispose the message writer and release the memory resource.
        /// It is the user's responsibility to call this method after finished accessing the message.
        /// </summary>
        public void Dispose()
        {
            Memory.free(buffer);
            buffer = null;
        }
    }
    
    /// <summary>
    /// Represents a read-only accessor on the message of type ErrorCodeResponse defined in the TSL protocols.
    /// The message readers will be instantiated by the system and passed to user's logic.
    /// After finished accessing the message. It is the user's responsibility to call Dispose()
    /// on the reader object. Recommend wrapping the reader with a <c>using Statement block</c>.
    /// <seealso ref="https://msdn.microsoft.com/en-us/library/yh598w02.aspx"/>
    /// </summary>
    public unsafe sealed class ErrorCodeResponseReader : ErrorCodeResponse_Accessor, IDisposable
    {
        byte * buffer;
        internal ErrorCodeResponseReader(byte* buf, int offset)
            : base(buf + offset
                   )
        {
            buffer = buf;
        }
        
        /// <summary>
        /// Dispose the message reader and release the memory resource.
        /// It is the user's responsibility to call this method after finished accessing the message.
        /// </summary>
        public void Dispose()
        {
            Memory.free(buffer);
            buffer = null;
        }
    }
    /// <summary>
    /// Represents a writer accessor on the message of type ErrorCodeResponse defined in the TSL protocols.
    /// The message writers should be instantiated by the user's logic and passed to the system to send it out.
    /// After finished accessing the message. It is the user's responsibility to call Dispose()
    /// on the writer object. Recommend wrapping the reader with a <c>using Statement block</c>.
    /// </summary>
    /// <seealso ref="https://msdn.microsoft.com/en-us/library/yh598w02.aspx"/>
    /// <remarks>Calling <c>Dispose()</c> does not send the message out.</remarks>
    public unsafe sealed class ErrorCodeResponseWriter : ErrorCodeResponse_Accessor, IDisposable
    {
        internal byte* buffer = null;
        internal int BufferLength;
        internal int Length; 
        public unsafe ErrorCodeResponseWriter( int errno = default(int) )
            : base(null
                   )
        {
            int preservedHeaderLength = TrinityProtocol.MsgHeader;
            
            byte* targetPtr;
            
            targetPtr = (byte*)preservedHeaderLength;
            targetPtr += 4;

            byte* tmpcellptr = (byte*)Memory.malloc((ulong)targetPtr);
            {
                BufferLength     = (int)targetPtr;
                Memory.memset(tmpcellptr, 0, (ulong)targetPtr);
                targetPtr = tmpcellptr;
                tmpcellptr += preservedHeaderLength;
                targetPtr  += preservedHeaderLength;
                
            {
            *(int*)targetPtr = errno;
            targetPtr += 4;

            }
            }
            
            buffer = tmpcellptr - preservedHeaderLength;
            this.m_ptr = buffer + preservedHeaderLength;
            Length = BufferLength - preservedHeaderLength;
            
        }
        internal unsafe ErrorCodeResponseWriter(int asyncRspHeaderLength,  int errno = default(int) )
            : base(null
                   )
        {
            int preservedHeaderLength = TrinityProtocol.MsgHeader + asyncRspHeaderLength;
            
            byte* targetPtr;
            
            targetPtr = (byte*)preservedHeaderLength;
            targetPtr += 4;

            byte* tmpcellptr = (byte*)Memory.malloc((ulong)targetPtr);
            {
                BufferLength     = (int)targetPtr;
                Memory.memset(tmpcellptr, 0, (ulong)targetPtr);
                targetPtr = tmpcellptr;
                tmpcellptr += preservedHeaderLength;
                targetPtr  += preservedHeaderLength;
                
            {
            *(int*)targetPtr = errno;
            targetPtr += 4;

            }
            }
            
            buffer = tmpcellptr - preservedHeaderLength;
            this.m_ptr = buffer + preservedHeaderLength;
            Length = BufferLength - preservedHeaderLength;
            
        }
        
        /// <summary>
        /// Dispose the message writer and release the memory resource.
        /// It is the user's responsibility to call this method after finished accessing the message.
        /// </summary>
        public void Dispose()
        {
            Memory.free(buffer);
            buffer = null;
        }
    }
    
}

#pragma warning restore 162,168,649,660,661,1522

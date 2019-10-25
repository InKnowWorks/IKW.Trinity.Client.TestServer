#pragma warning disable 162,168,649,660,661,1522
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System.Security;
using Trinity;
using Trinity.Core.Lib;
using Trinity.Storage;
using Trinity.Utilities;
using Trinity.TSL.Lib;
using Trinity.Network;
using Trinity.Network.Sockets;
using Trinity.Network.Messaging;
using Trinity.TSL;
using System.Runtime.CompilerServices;
using Trinity.Storage.Transaction;
using Microsoft.Extensions.ObjectPool;
namespace Trinity.Client.TestProtocols
{
    
    /// <summary>
    /// A .NET runtime object representation of TripleStore defined in TSL.
    /// </summary>
    public partial struct TripleStore : ICell
    {
        ///<summary>
        ///The id of the cell.
        ///</summary>
        public long CellId;
        ///<summary>
        ///Initializes a new instance of TripleStore with the specified parameters.
        ///</summary>
        public TripleStore(long cell_id , Triple TripleCell = default(Triple))
        {
            
            this.TripleCell = TripleCell;
            
            CellId = cell_id;
        }
        
        ///<summary>
        ///Initializes a new instance of TripleStore with the specified parameters.
        ///</summary>
        public TripleStore( Triple TripleCell = default(Triple))
        {
            
            this.TripleCell = TripleCell;
            
            CellId = CellIdFactory.NewCellId();
        }
        
        public Triple TripleCell;
        
        public static bool operator ==(TripleStore a, TripleStore b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            
            return
                
                (a.TripleCell == b.TripleCell)
                
                ;
            
        }
        public static bool operator !=(TripleStore a, TripleStore b)
        {
            return !(a == b);
        }
        #region Text processing
        /// <summary>
        /// Converts the string representation of a TripleStore to its
        /// struct equivalent. A return value indicates whether the 
        /// operation succeeded.
        /// </summary>
        /// <param name="input">A string to convert.</param>
        /// <param name="value">
        /// When this method returns, contains the struct equivalent of the value contained 
        /// in input, if the conversion succeeded, or default(TripleStore) if the conversion
        /// failed. The conversion fails if the input parameter is null or String.Empty, or is 
        /// not of the correct format. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// True if input was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string input, out TripleStore value)
        {
            try
            {
                value = Newtonsoft.Json.JsonConvert.DeserializeObject<TripleStore>(input);
                return true;
            }
            catch { value = default(TripleStore); return false; }
        }
        public static TripleStore Parse(string input)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TripleStore>(input);
        }
        ///<summary>Converts a TripleStore to its string representation, in JSON format.</summary>
        ///<returns>A string representation of the TripleStore.</returns>
        public override string ToString()
        {
            return Serializer.ToString(this);
        }
        #endregion
        #region Lookup tables
        internal static StringLookupTable FieldLookupTable = new StringLookupTable(
            
            "TripleCell"
            
            );
        internal static HashSet<string> AppendToFieldRerouteSet = new HashSet<string>()
        {
            
            "TripleCell"
            ,
        };
        #endregion
        #region ICell implementation
        /// <summary>
        /// Get the field of the specified name in the cell.
        /// </summary>
        /// <typeparam name="T">
        /// The desired type that the field is supposed 
        /// to be interpreted as. Automatic type casting 
        /// will be attempted if the desired type is not 
        /// implicitly convertible from the type of the field.
        /// </typeparam>
        /// <param name="fieldName">The name of the target field.</param>
        /// <returns>The value of the field.</returns>
        public T GetField<T>(string fieldName)
        {
            switch (FieldLookupTable.Lookup(fieldName))
            {
                case -1:
                Throw.undefined_field();
                break;
                
                case 0:
                return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                
            }
            /* Should not reach here */
            throw new Exception("Internal error T5005");
        }
        /// <summary>
        /// Set the value of the target field.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <param name="fieldName">The name of the target field.</param>
        /// <param name="value">
        /// The value of the field. Automatic type casting 
        /// will be attempted if the desired type is not 
        /// implicitly convertible from the type of the field.
        /// </param>
        public void SetField<T>(string fieldName, T value)
        {
            switch (FieldLookupTable.Lookup(fieldName))
            {
                case -1:
                Throw.undefined_field();
                break;
                
                case 0:
                this.TripleCell = TypeConverter<T>.ConvertTo_Triple(value);
                break;
                
                default:
                Throw.data_type_incompatible_with_field(typeof(T).ToString());
                break;
            }
        }
        /// <summary>
        /// Tells if a field with the given name exists in the current cell.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The existence of the field.</returns>
        public bool ContainsField(string fieldName)
        {
            switch (FieldLookupTable.Lookup(fieldName))
            {
                
                case 0:
                
                return true;
                
                default:
                return false;
            }
        }
        /// <summary>
        /// Append <paramref name="value"/> to the target field. Note that if the target field
        /// is not appendable(string or list), calling this method is equivalent to <see cref="Trinity.Client.TestProtocols.GenericCellAccessor.SetField(string, T)"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <param name="fieldName">The name of the target field.</param>
        /// <param name="value">The value to be appended. 
        /// If the value is incompatible with the element 
        /// type of the field, automatic type casting will be attempted.
        /// </param>
        public void AppendToField<T>(string fieldName, T value)
        {
            if (AppendToFieldRerouteSet.Contains(fieldName))
            {
                SetField(fieldName, value);
                return;
            }
            switch (FieldLookupTable.Lookup(fieldName))
            {
                case -1:
                Throw.undefined_field();
                break;
                
                default:
                Throw.target__field_not_list();
                break;
            }
        }
        long ICell.CellId { get { return CellId; } set { CellId = value; } }
        public IEnumerable<KeyValuePair<string, T>> SelectFields<T>(string attributeKey, string attributeValue)
        {
            switch (TypeConverter<T>.type_id)
            {
                
                case 1:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                case 2:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                case 3:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                case 4:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                default:
                Throw.incompatible_with_cell();
                break;
            }
            yield break;
        }
        #region enumerate value constructs
        
        private IEnumerable<T> _enumerate_from_TripleCell<T>()
        {
            
            switch (TypeConverter<T>.type_id)
            {
                
                case 1:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                case 2:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                case 3:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                case 4:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                default:
                    Throw.incompatible_with_cell();
                    break;
            }
            yield break;
            
        }
        
        private static StringLookupTable s_field_attribute_id_table = new StringLookupTable(
            
            );
        #endregion
        public IEnumerable<T> EnumerateField<T>(string fieldName)
        {
            switch (FieldLookupTable.Lookup(fieldName))
            {
                
                case 0:
                return _enumerate_from_TripleCell<T>();
                
                default:
                Throw.undefined_field();
                return null;
            }
        }
        public IEnumerable<T> EnumerateValues<T>(string attributeKey, string attributeValue)
        {
            int attr_id;
            if (attributeKey == null)
            {
                
                foreach (var val in _enumerate_from_TripleCell<T>())
                    yield return val;
                
            }
            else if (-1 != (attr_id = s_field_attribute_id_table.Lookup(attributeKey)))
            {
                switch (attr_id)
                {
                    
                }
            }
            yield break;
        }
        public ICellAccessor Serialize()
        {
            return (TripleStore_Accessor)this;
        }
        #endregion
        #region Other interfaces
        string ITypeDescriptor.TypeName
        {
            get { return StorageSchema.s_cellTypeName_TripleStore; }
        }
        Type ITypeDescriptor.Type
        {
            get { return StorageSchema.s_cellType_TripleStore; }
        }
        bool ITypeDescriptor.IsOfType<T>()
        {
            return typeof(T) == StorageSchema.s_cellType_TripleStore;
        }
        bool ITypeDescriptor.IsList()
        {
            return false;
        }
        IEnumerable<IFieldDescriptor> ICellDescriptor.GetFieldDescriptors()
        {
            return StorageSchema.TripleStore.GetFieldDescriptors();
        }
        IAttributeCollection ICellDescriptor.GetFieldAttributes(string fieldName)
        {
            return StorageSchema.TripleStore.GetFieldAttributes(fieldName);
        }
        string IAttributeCollection.GetAttributeValue(string attributeKey)
        {
            return StorageSchema.TripleStore.GetAttributeValue(attributeKey);
        }
        IReadOnlyDictionary<string, string> IAttributeCollection.Attributes
        {
            get { return StorageSchema.TripleStore.Attributes; }
        }
        IEnumerable<string> ICellDescriptor.GetFieldNames()
        {
            
            {
                yield return "TripleCell";
            }
            
        }
        ushort ICellDescriptor.CellType
        {
            get
            {
                return (ushort)CellType.TripleStore;
            }
        }
        #endregion
    }
    /// <summary>
    /// Provides in-place operations of TripleStore defined in TSL.
    /// </summary>
    public unsafe class TripleStore_Accessor : ICellAccessor
    {
        #region Fields
        internal   long                    m_cellId;
        /// <summary>
        /// A pointer to the underlying raw binary blob. Take caution when accessing data with
        /// the raw pointer, as no boundary checks are employed, and improper operations will cause data corruption and/or system crash.
        /// </summary>
        internal byte*                   m_ptr;
        internal LocalTransactionContext m_tx;
        internal int                     m_cellEntryIndex;
        internal CellAccessOptions       m_options;
        internal bool                    m_IsIterator;
        private  const CellAccessOptions c_WALFlags = CellAccessOptions.StrongLogAhead | CellAccessOptions.WeakLogAhead;
        #endregion
        #region Constructors
        private unsafe TripleStore_Accessor()
        {
                    TripleCell_Accessor_Field = new Triple_Accessor(null,
                (ptr,ptr_offset,delta)=>
                {
                    int substructure_offset = (int)(ptr - this.m_ptr);
                    this.ResizeFunction(this.m_ptr, ptr_offset + substructure_offset, delta);
                    return this.m_ptr + substructure_offset;
                });
        }
        #endregion
        
        internal static string[] optional_field_names = null;
        ///<summary>
        ///Get an array of the names of all optional fields for object type t_struct_name.
        ///</summary>
        public static string[] GetOptionalFieldNames()
        {
            if (optional_field_names == null)
                optional_field_names = new string[]
                {
                    
                };
            return optional_field_names;
        }
        ///<summary>
        ///Get a list of the names of available optional fields in the object being operated by this accessor.
        ///</summary>
        internal List<string> GetNotNullOptionalFields()
        {
            List<string> list = new List<string>();
            BitArray ba = new BitArray(GetOptionalFieldMap());
            string[] optional_fields = GetOptionalFieldNames();
            for (int i = 0; i < ba.Count; i++)
            {
                if (ba[i])
                    list.Add(optional_fields[i]);
            }
            return list;
        }
        internal unsafe byte[] GetOptionalFieldMap()
        {
            
            return new byte[0];
            
        }
        
        #region IAccessor Implementation
        public byte[] ToByteArray()
        {
            byte* targetPtr = m_ptr;
            {{targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);}}
            int size = (int)(targetPtr - m_ptr);
            byte[] ret = new byte[size];
            Memory.Copy(m_ptr, 0, ret, 0, size);
            return ret;
        }
        public unsafe byte* GetUnderlyingBufferPointer()
        {
            return m_ptr;
        }
        public unsafe int GetBufferLength()
        {
            byte* targetPtr = m_ptr;
            {{targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);}}
            int size = (int)(targetPtr - m_ptr);
            return size;
        }
        public ResizeFunctionDelegate ResizeFunction { get; set; }
        #endregion
        private static byte[] s_default_content = null;
        private static unsafe byte[] construct( Triple TripleCell = default(Triple) )
        {
            if (s_default_content != null) return s_default_content;
            
            byte* targetPtr;
            
            targetPtr = null;
            
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
            }
            byte[] tmpcell = new byte[(int)(targetPtr)];
            fixed (byte* _tmpcellptr = tmpcell)
            {
                targetPtr = _tmpcellptr;
                
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Subject)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Predicate)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Object)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Namespace)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
            }
            }
            
            s_default_content = tmpcell;
            return tmpcell;
        }
        Triple_Accessor TripleCell_Accessor_Field;
        
        ///<summary>
        ///Provides in-place access to the object field TripleCell.
        ///</summary>
        public unsafe Triple_Accessor TripleCell
        {
            get
            {
                
                byte* targetPtr = m_ptr;
                {}TripleCell_Accessor_Field.m_ptr = targetPtr;
                TripleCell_Accessor_Field.m_cellId = this.m_cellId;
                return TripleCell_Accessor_Field;
                
            }
            set
            {
                
                if ((object)value == null) throw new ArgumentNullException("The assigned variable is null.");
                TripleCell_Accessor_Field.m_cellId = this.m_cellId;
                
                byte* targetPtr = m_ptr;
                {}
                int offset = (int)(targetPtr - m_ptr);
                byte* oldtargetPtr = targetPtr;
                int oldlength = (int)(targetPtr - oldtargetPtr);
                targetPtr = value.m_ptr;
                int newlength = (int)(targetPtr - value.m_ptr);
                if (newlength != oldlength)
                {
                    if (value.m_cellId != this.m_cellId)
                    {
                        this.m_ptr = this.ResizeFunction(this.m_ptr, offset, newlength - oldlength);
                        Memory.Copy(value.m_ptr, this.m_ptr + offset, newlength);
                    }
                    else
                    {
                        byte[] tmpcell = new byte[newlength];
                        fixed(byte* tmpcellptr = tmpcell)
                        {
                            Memory.Copy(value.m_ptr, tmpcellptr, newlength);
                            this.m_ptr = this.ResizeFunction(this.m_ptr, offset, newlength - oldlength);
                            Memory.Copy(tmpcellptr, this.m_ptr + offset, newlength);
                        }
                    }
                }
                else
                {
                    Memory.Copy(value.m_ptr, oldtargetPtr, oldlength);
                }
            }
        }
        
        public static unsafe implicit operator TripleStore(TripleStore_Accessor accessor)
        {
            
            return new TripleStore(accessor.CellId
            
            ,
            
                    accessor.TripleCell
            );
        }
        
        public unsafe static implicit operator TripleStore_Accessor(TripleStore field)
        {
            byte* targetPtr = null;
            
            {

            {

        if(field.TripleCell.Subject!= null)
        {
            int strlen_3 = field.TripleCell.Subject.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(field.TripleCell.Predicate!= null)
        {
            int strlen_3 = field.TripleCell.Predicate.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(field.TripleCell.Object!= null)
        {
            int strlen_3 = field.TripleCell.Object.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(field.TripleCell.Namespace!= null)
        {
            int strlen_3 = field.TripleCell.Namespace.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
            }
            byte* tmpcellptr = BufferAllocator.AllocBuffer((int)targetPtr);
            Memory.memset(tmpcellptr, 0, (ulong)targetPtr);
            targetPtr = tmpcellptr;
            
            {

            {

        if(field.TripleCell.Subject!= null)
        {
            int strlen_3 = field.TripleCell.Subject.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = field.TripleCell.Subject)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(field.TripleCell.Predicate!= null)
        {
            int strlen_3 = field.TripleCell.Predicate.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = field.TripleCell.Predicate)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(field.TripleCell.Object!= null)
        {
            int strlen_3 = field.TripleCell.Object.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = field.TripleCell.Object)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(field.TripleCell.Namespace!= null)
        {
            int strlen_3 = field.TripleCell.Namespace.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = field.TripleCell.Namespace)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
            }TripleStore_Accessor ret;
            
            ret = TripleStore_Accessor._get()._Setup(field.CellId, tmpcellptr, -1, 0, null);
            ret.m_cellId = field.CellId;
            
            return ret;
        }
        
        public static bool operator ==(TripleStore_Accessor a, TripleStore_Accessor b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;
            if (a.m_ptr == b.m_ptr) return true;
            byte* targetPtr = a.m_ptr;
            {{targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);}}
            int lengthA = (int)(targetPtr - a.m_ptr);
            targetPtr = b.m_ptr;
            {{targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);targetPtr += *(int*)targetPtr + sizeof(int);}}
            int lengthB = (int)(targetPtr - b.m_ptr);
            if(lengthA != lengthB) return false;
            return Memory.Compare(a.m_ptr,b.m_ptr,lengthA);
        }
        public static bool operator != (TripleStore_Accessor a, TripleStore_Accessor b)
        {
            return !(a == b);
        }
        
        public static bool operator ==(TripleStore_Accessor a, TripleStore b)
        {
            TripleStore_Accessor bb = b;
            return (a == bb);
        }
        public static bool operator !=(TripleStore_Accessor a, TripleStore b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Get the size of the cell content, in bytes.
        /// </summary>
        public int CellSize { get { int size; Global.LocalStorage.LockedGetCellSize(this.CellId, this.m_cellEntryIndex, out size); return size; } }
        #region Internal
        private unsafe byte* _Resize_NonTx(byte* ptr, int ptr_offset, int delta)
        {
            int offset = (int)(ptr - m_ptr) + ptr_offset;
            m_ptr = Global.LocalStorage.ResizeCell((long)CellId, m_cellEntryIndex, offset, delta);
            return m_ptr + (offset - ptr_offset);
        }
        private unsafe byte* _Resize_Tx(byte* ptr, int ptr_offset, int delta)
        {
            int offset = (int)(ptr - m_ptr) + ptr_offset;
            m_ptr = Global.LocalStorage.ResizeCell(m_tx, (long)CellId, m_cellEntryIndex, offset, delta);
            return m_ptr + (offset - ptr_offset);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe TripleStore_Accessor _Lock(long cellId, CellAccessOptions options)
        {
            ushort cellType;
            this.CellId = cellId;
            this.m_options = options;
            this.ResizeFunction = _Resize_NonTx;
            TrinityErrorCode eResult = Global.LocalStorage.GetLockedCellInfo(cellId, out _, out cellType, out this.m_ptr, out this.m_cellEntryIndex);
            switch (eResult)
            {
                case TrinityErrorCode.E_SUCCESS:
                {
                    if (cellType != (ushort)CellType.TripleStore)
                    {
                        Global.LocalStorage.ReleaseCellLock(cellId, this.m_cellEntryIndex);
                        _put(this);
                        Throw.wrong_cell_type();
                    }
                    break;
                }
                case TrinityErrorCode.E_CELL_NOT_FOUND:
                {
                    if ((options & CellAccessOptions.ThrowExceptionOnCellNotFound) != 0)
                    {
                        _put(this);
                        Throw.cell_not_found(cellId);
                    }
                    else if ((options & CellAccessOptions.CreateNewOnCellNotFound) != 0)
                    {
                        byte[]  defaultContent = construct();
                        int     size           = defaultContent.Length;
                        eResult                = Global.LocalStorage.AddOrUse(cellId, defaultContent, ref size, (ushort)CellType.TripleStore, out this.m_ptr, out this.m_cellEntryIndex);
                        if (eResult == TrinityErrorCode.E_WRONG_CELL_TYPE)
                        {
                            _put(this);
                            Throw.wrong_cell_type();
                        }
                    }
                    else if ((options & CellAccessOptions.ReturnNullOnCellNotFound) != 0)
                    {
                        _put(this);
                        return null;
                    }
                    else
                    {
                        _put(this);
                        Throw.cell_not_found(cellId);
                    }
                    break;
                }
                default:
                _put(this);
                throw new NotImplementedException();
            }
            return this;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe TripleStore_Accessor _Lock(long cellId, CellAccessOptions options, LocalTransactionContext tx)
        {
            ushort cellType;
            this.CellId = cellId;
            this.m_options = options;
            this.m_tx = tx;
            this.ResizeFunction = _Resize_Tx;
            TrinityErrorCode eResult = Global.LocalStorage.GetLockedCellInfo(tx, cellId, out _, out cellType, out this.m_ptr, out this.m_cellEntryIndex);
            switch (eResult)
            {
                case TrinityErrorCode.E_SUCCESS:
                {
                    if (cellType != (ushort)CellType.TripleStore)
                    {
                        Global.LocalStorage.ReleaseCellLock(tx, cellId, this.m_cellEntryIndex);
                        _put(this);
                        Throw.wrong_cell_type();
                    }
                    break;
                }
                case TrinityErrorCode.E_CELL_NOT_FOUND:
                {
                    if ((options & CellAccessOptions.ThrowExceptionOnCellNotFound) != 0)
                    {
                        _put(this);
                        Throw.cell_not_found(cellId);
                    }
                    else if ((options & CellAccessOptions.CreateNewOnCellNotFound) != 0)
                    {
                        byte[]  defaultContent = construct();
                        int     size           = defaultContent.Length;
                        eResult                = Global.LocalStorage.AddOrUse(tx, cellId, defaultContent, ref size, (ushort)CellType.TripleStore, out this.m_ptr, out this.m_cellEntryIndex);
                        if (eResult == TrinityErrorCode.E_WRONG_CELL_TYPE)
                        {
                            _put(this);
                            Throw.wrong_cell_type();
                        }
                    }
                    else if ((options & CellAccessOptions.ReturnNullOnCellNotFound) != 0)
                    {
                        _put(this);
                        return null;
                    }
                    else
                    {
                        _put(this);
                        Throw.cell_not_found(cellId);
                    }
                    break;
                }
                default:
                _put(this);
                throw new NotImplementedException();
            }
            return this;
        }
        private class PoolPolicy : IPooledObjectPolicy<TripleStore_Accessor>
        {
            public TripleStore_Accessor Create()
            {
                return new TripleStore_Accessor();
            }
            public bool Return(TripleStore_Accessor obj)
            {
                return !obj.m_IsIterator;
            }
        }
        private static DefaultObjectPool<TripleStore_Accessor> s_accessor_pool = new DefaultObjectPool<TripleStore_Accessor>(new PoolPolicy());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TripleStore_Accessor _get() => s_accessor_pool.Get();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void _put(TripleStore_Accessor item) => s_accessor_pool.Return(item);
        /// <summary>
        /// For internal use only.
        /// Caller guarantees that entry lock is obtained.
        /// Does not handle CellAccessOptions. Only copy to the accessor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal TripleStore_Accessor _Setup(long CellId, byte* cellPtr, int entryIndex, CellAccessOptions options)
        {
            this.CellId      = CellId;
            m_cellEntryIndex = entryIndex;
            m_options        = options;
            m_ptr            = cellPtr;
            m_tx             = null;
            this.ResizeFunction = _Resize_NonTx;
            return this;
        }
        /// <summary>
        /// For internal use only.
        /// Caller guarantees that entry lock is obtained.
        /// Does not handle CellAccessOptions. Only copy to the accessor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal TripleStore_Accessor _Setup(long CellId, byte* cellPtr, int entryIndex, CellAccessOptions options, LocalTransactionContext tx)
        {
            this.CellId      = CellId;
            m_cellEntryIndex = entryIndex;
            m_options        = options;
            m_ptr            = cellPtr;
            m_tx             = tx;
            this.ResizeFunction = _Resize_Tx;
            return this;
        }
        /// <summary>
        /// For internal use only.
        /// </summary>
        internal static TripleStore_Accessor AllocIterativeAccessor(CellInfo info, LocalTransactionContext tx)
        {
            TripleStore_Accessor accessor = new TripleStore_Accessor();
            accessor.m_IsIterator = true;
            if (tx != null) accessor._Setup(info.CellId, info.CellPtr, info.CellEntryIndex, 0, tx);
            else accessor._Setup(info.CellId, info.CellPtr, info.CellEntryIndex, 0);
            return accessor;
        }
        #endregion
        #region Public
        /// <summary>
        /// Dispose the accessor.
        /// If <c><see cref="Trinity.TrinityConfig.ReadOnly"/> == false</c>,
        /// the cell lock will be released.
        /// If write-ahead-log behavior is specified on <see cref="Trinity.Client.TestProtocols.StorageExtension_TripleStore.UseTripleStore"/>,
        /// the changes will be committed to the write-ahead log.
        /// </summary>
        public void Dispose()
        {
            if (m_cellEntryIndex >= 0)
            {
                if ((m_options & c_WALFlags) != 0)
                {
                    LocalMemoryStorage.CWriteAheadLog(this.CellId, this.m_ptr, this.CellSize, (ushort)CellType.TripleStore, m_options);
                }
                if (!m_IsIterator)
                {
                    if (m_tx == null) Global.LocalStorage.ReleaseCellLock(CellId, m_cellEntryIndex);
                    else Global.LocalStorage.ReleaseCellLock(m_tx, CellId, m_cellEntryIndex);
                }
            }
            _put(this);
        }
        /// <summary>
        /// Get the cell type id.
        /// </summary>
        /// <returns>A 16-bit unsigned integer representing the cell type id.</returns>
        public ushort GetCellType()
        {
            ushort cellType;
            if (Global.LocalStorage.GetCellType(CellId, out cellType) == TrinityErrorCode.E_CELL_NOT_FOUND)
            {
                Throw.cell_not_found();
            }
            return cellType;
        }
        /// <summary>Converts a TripleStore_Accessor to its string representation, in JSON format.</summary>
        /// <returns>A string representation of the TripleStore.</returns>
        public override string ToString()
        {
            return Serializer.ToString(this);
        }
        #endregion
        #region Lookup tables
        internal static StringLookupTable FieldLookupTable = new StringLookupTable(
            
            "TripleCell"
            
            );
        static HashSet<string> AppendToFieldRerouteSet = new HashSet<string>()
        {
            
            "TripleCell"
            ,
        };
        #endregion
        #region ICell implementation
        public T GetField<T>(string fieldName)
        {
            int field_divider_idx = fieldName.IndexOf('.');
            if (-1 != field_divider_idx)
            {
                string field_name_string = fieldName.Substring(0, field_divider_idx);
                switch (FieldLookupTable.Lookup(field_name_string))
                {
                    case -1:
                    Throw.undefined_field();
                    break;
                    
                    case 0:
                    return GenericFieldAccessor.GetField<T>(this.TripleCell, fieldName, field_divider_idx + 1);
                    
                    default:
                    Throw.member_access_on_non_struct__field(field_name_string);
                    break;
                }
            }
            switch (FieldLookupTable.Lookup(fieldName))
            {
                case -1:
                Throw.undefined_field();
                break;
                
                case 0:
                return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                
            }
            /* Should not reach here */
            throw new Exception("Internal error T5005");
        }
        public void SetField<T>(string fieldName, T value)
        {
            int field_divider_idx = fieldName.IndexOf('.');
            if (-1 != field_divider_idx)
            {
                string field_name_string = fieldName.Substring(0, field_divider_idx);
                switch (FieldLookupTable.Lookup(field_name_string))
                {
                    case -1:
                    Throw.undefined_field();
                    break;
                    
                    case 0:
                    GenericFieldAccessor.SetField(this.TripleCell, fieldName, field_divider_idx + 1, value);
                    break;
                    
                    default:
                    Throw.member_access_on_non_struct__field(field_name_string);
                    break;
                }
                return;
            }
            switch (FieldLookupTable.Lookup(fieldName))
            {
                case -1:
                Throw.undefined_field();
                break;
                
                case 0:
                {
                    Triple conversion_result = TypeConverter<T>.ConvertTo_Triple(value);
                    
            {
                this.TripleCell = conversion_result;
            }
            
                }
                break;
                
            }
        }
        /// <summary>
        /// Tells if a field with the given name exists in the current cell.
        /// </summary>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The existence of the field.</returns>
        public bool ContainsField(string fieldName)
        {
            switch (FieldLookupTable.Lookup(fieldName))
            {
                
                case 0:
                
                return true;
                
                default:
                return false;
            }
        }
        public void AppendToField<T>(string fieldName, T value)
        {
            if (AppendToFieldRerouteSet.Contains(fieldName))
            {
                SetField(fieldName, value);
                return;
            }
            switch (FieldLookupTable.Lookup(fieldName))
            {
                case -1:
                Throw.undefined_field();
                break;
                
                default:
                Throw.target__field_not_list();
                break;
            }
        }
        public long CellId { get { return m_cellId; } set { m_cellId = value; } }
        IEnumerable<KeyValuePair<string, T>> ICell.SelectFields<T>(string attributeKey, string attributeValue)
        {
            switch (TypeConverter<T>.type_id)
            {
                
                case 1:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                case 2:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                case 3:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                case 4:
                
                if (StorageSchema.TripleStore_descriptor.check_attribute(StorageSchema.TripleStore_descriptor.TripleCell, attributeKey, attributeValue))
                    
                        yield return new KeyValuePair<string, T>("TripleCell", TypeConverter<T>.ConvertFrom_Triple(this.TripleCell));
                
                break;
                
                default:
                Throw.incompatible_with_cell();
                break;
            }
            yield break;
        }
        #region enumerate value methods
        
        private IEnumerable<T> _enumerate_from_TripleCell<T>()
        {
            
            switch (TypeConverter<T>.type_id)
            {
                
                case 1:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                case 2:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                case 3:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                case 4:
                    {
                        
                        yield return TypeConverter<T>.ConvertFrom_Triple(this.TripleCell);
                        
                    }
                    break;
                
                default:
                    Throw.incompatible_with_cell();
                    break;
            }
            yield break;
            
        }
        
        private static StringLookupTable s_field_attribute_id_table = new StringLookupTable(
            
            );
        #endregion
        public IEnumerable<T> EnumerateField<T>(string fieldName)
        {
            switch (FieldLookupTable.Lookup(fieldName))
            {
                
                case 0:
                return _enumerate_from_TripleCell<T>();
                
                default:
                Throw.undefined_field();
                return null;
            }
        }
        IEnumerable<T> ICell.EnumerateValues<T>(string attributeKey, string attributeValue)
        {
            int attr_id;
            if (attributeKey == null)
            {
                
                foreach (var val in _enumerate_from_TripleCell<T>())
                    yield return val;
                
            }
            else if (-1 != (attr_id = s_field_attribute_id_table.Lookup(attributeKey)))
            {
                switch (attr_id)
                {
                    
                }
            }
            yield break;
        }
        IEnumerable<string> ICellDescriptor.GetFieldNames()
        {
            
            {
                yield return "TripleCell";
            }
            
        }
        IAttributeCollection ICellDescriptor.GetFieldAttributes(string fieldName)
        {
            return StorageSchema.TripleStore.GetFieldAttributes(fieldName);
        }
        IEnumerable<IFieldDescriptor> ICellDescriptor.GetFieldDescriptors()
        {
            return StorageSchema.TripleStore.GetFieldDescriptors();
        }
        string ITypeDescriptor.TypeName
        {
            get { return StorageSchema.s_cellTypeName_TripleStore; }
        }
        Type ITypeDescriptor.Type
        {
            get { return StorageSchema.s_cellType_TripleStore; }
        }
        bool ITypeDescriptor.IsOfType<T>()
        {
            return typeof(T) == StorageSchema.s_cellType_TripleStore;
        }
        bool ITypeDescriptor.IsList()
        {
            return false;
        }
        IReadOnlyDictionary<string, string> IAttributeCollection.Attributes
        {
            get { return StorageSchema.TripleStore.Attributes; }
        }
        string IAttributeCollection.GetAttributeValue(string attributeKey)
        {
            return StorageSchema.TripleStore.GetAttributeValue(attributeKey);
        }
        ushort ICellDescriptor.CellType
        {
            get
            {
                return (ushort)CellType.TripleStore;
            }
        }
        public ICellAccessor Serialize()
        {
            return this;
        }
        #endregion
        public ICell Deserialize()
        {
            return (TripleStore)this;
        }
    }
    ///<summary>
    ///Provides interfaces for accessing TripleStore cells
    ///on <see cref="Trinity.Storage.LocalMemorySotrage"/>.
    static public class StorageExtension_TripleStore
    {
        #region IKeyValueStore non logging
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. The value of the cell is specified in the method parameters.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.IKeyValueStore"/> instance.</param>
        /// <param name="cellId">A 64-bit cell Id.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this IKeyValueStore storage, long cellId, Triple TripleCell = default(Triple))
        {
            
            byte* targetPtr;
            
            targetPtr = null;
            
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
            }
            byte[] tmpcell = new byte[(int)(targetPtr)];
            fixed (byte* _tmpcellptr = tmpcell)
            {
                targetPtr = _tmpcellptr;
                
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Subject)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Predicate)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Object)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Namespace)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
            }
            }
            
            return storage.SaveCell(cellId, tmpcell, (ushort)CellType.TripleStore) == TrinityErrorCode.E_SUCCESS;
        }
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. The parameter <paramref name="cellId"/> overrides the cell id in the content object.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.IKeyValueStore"/> instance.</param>
        /// <param name="cellId">A 64-bit cell Id.</param>
        /// <param name="cellContent">The content of the cell.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this IKeyValueStore storage, long cellId, TripleStore cellContent)
        {
            return SaveTripleStore(storage, cellId  , cellContent.TripleCell );
        }
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. Cell Id is specified by the CellId field in the content object.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.IKeyValueStore"/> instance.</param>
        /// <param name="cellContent">The content of the cell.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this IKeyValueStore storage, TripleStore cellContent)
        {
            return SaveTripleStore(storage, cellContent.CellId  , cellContent.TripleCell );
        }
        /// <summary>
        /// Loads the content of the specified cell. Any changes done to this object are not written to the store, unless
        /// the content object is saved back into the storage.
        /// <param name="storage"/>A <see cref="Trinity.Storage.IKeyValueStore"/> instance.</param>
        /// </summary>
        public unsafe static TripleStore LoadTripleStore(this IKeyValueStore storage, long cellId)
        {
            if (TrinityErrorCode.E_SUCCESS == storage.LoadCell(cellId, out var buff))
            {
                fixed (byte* p = buff)
                {
                    return TripleStore_Accessor._get()._Setup(cellId, p, -1, 0);
                }
            }
            else
            {
                Throw.cell_not_found();
                throw new Exception();
            }
        }
        #endregion
        #region LocalMemoryStorage Non-Tx accessors
        /// <summary>
        /// Allocate a cell accessor on the specified cell, which interprets
        /// the cell as a TripleStore. Any changes done to the accessor
        /// are written to the storage immediately.
        /// If <c><see cref="Trinity.TrinityConfig.ReadOnly"/> == false</c>,
        /// on calling this method, it attempts to acquire the lock of the cell,
        /// and blocks until it gets the lock. Otherwise this method is wait-free.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">The id of the specified cell.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <returns>A <see cref="Trinity.Client.TestProtocols.TripleStore"/> instance.</returns>
        public unsafe static TripleStore_Accessor UseTripleStore(this Trinity.Storage.LocalMemoryStorage storage, long cellId, CellAccessOptions options)
        {
            return TripleStore_Accessor._get()._Lock(cellId, options);
        }
        /// <summary>
        /// Allocate a cell accessor on the specified cell, which interprets
        /// the cell as a TripleStore. Any changes done to the accessor
        /// are written to the storage immediately.
        /// If <c><see cref="Trinity.TrinityConfig.ReadOnly"/> == false</c>,
        /// on calling this method, it attempts to acquire the lock of the cell,
        /// and blocks until it gets the lock.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">The id of the specified cell.</param>
        /// <returns>A <see cref="" + script.RootNamespace + ".TripleStore"/> instance.</returns>
        public unsafe static TripleStore_Accessor UseTripleStore(this Trinity.Storage.LocalMemoryStorage storage, long cellId)
        {
            return TripleStore_Accessor._get()._Lock(cellId, CellAccessOptions.ThrowExceptionOnCellNotFound);
        }
        #endregion
        #region LocalStorage Non-Tx logging
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. The value of the cell is specified in the method parameters.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">A 64-bit cell Id.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this Trinity.Storage.LocalMemoryStorage storage, CellAccessOptions options, long cellId, Triple TripleCell = default(Triple))
        {
            
            byte* targetPtr;
            
            targetPtr = null;
            
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
            }
            byte[] tmpcell = new byte[(int)(targetPtr)];
            fixed (byte* _tmpcellptr = tmpcell)
            {
                targetPtr = _tmpcellptr;
                
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Subject)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Predicate)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Object)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Namespace)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
            }
            }
            
            return storage.SaveCell(options, cellId, tmpcell, 0, tmpcell.Length, (ushort)CellType.TripleStore) == TrinityErrorCode.E_SUCCESS;
        }
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. The parameter <paramref name="cellId"/> overrides the cell id in the content object.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">A 64-bit cell Id.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <param name="cellContent">The content of the cell.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this Trinity.Storage.LocalMemoryStorage storage, CellAccessOptions options, long cellId, TripleStore cellContent)
        {
            return SaveTripleStore(storage, options, cellId  , cellContent.TripleCell );
        }
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. Cell Id is specified by the CellId field in the content object.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <param name="cellContent">The content of the cell.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this Trinity.Storage.LocalMemoryStorage storage, CellAccessOptions options, TripleStore cellContent)
        {
            return SaveTripleStore(storage, options, cellContent.CellId  , cellContent.TripleCell );
        }
        /// <summary>
        /// Loads the content of the specified cell. Any changes done to this object are not written to the store, unless
        /// the content object is saved back into the storage.
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// </summary>
        public unsafe static TripleStore LoadTripleStore(this Trinity.Storage.LocalMemoryStorage storage, long cellId)
        {
            using (var cell = TripleStore_Accessor._get()._Lock(cellId, CellAccessOptions.ThrowExceptionOnCellNotFound))
            {
                return cell;
            }
        }
        #endregion
        #region LocalMemoryStorage Tx accessors
        /// <summary>
        /// Allocate a cell accessor on the specified cell, which interprets
        /// the cell as a TripleStore. Any changes done to the accessor
        /// are written to the storage immediately.
        /// If <c><see cref="Trinity.TrinityConfig.ReadOnly"/> == false</c>,
        /// on calling this method, it attempts to acquire the lock of the cell,
        /// and blocks until it gets the lock. Otherwise this method is wait-free.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">The id of the specified cell.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <returns>A <see cref="Trinity.Client.TestProtocols.TripleStore"/> instance.</returns>
        public unsafe static TripleStore_Accessor UseTripleStore(this Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx, long cellId, CellAccessOptions options)
        {
            return TripleStore_Accessor._get()._Lock(cellId, options, tx);
        }
        /// <summary>
        /// Allocate a cell accessor on the specified cell, which interprets
        /// the cell as a TripleStore. Any changes done to the accessor
        /// are written to the storage immediately.
        /// If <c><see cref="Trinity.TrinityConfig.ReadOnly"/> == false</c>,
        /// on calling this method, it attempts to acquire the lock of the cell,
        /// and blocks until it gets the lock.
        /// </summary>
        /// <param name="storage">A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">The id of the specified cell.</param>
        /// <returns>A <see cref="" + script.RootNamespace + ".TripleStore"/> instance.</returns>
        public unsafe static TripleStore_Accessor UseTripleStore(this Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx, long cellId)
        {
            return TripleStore_Accessor._get()._Lock(cellId, CellAccessOptions.ThrowExceptionOnCellNotFound, tx);
        }
        #endregion
        #region LocalStorage Tx logging
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. The value of the cell is specified in the method parameters.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">A 64-bit cell Id.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx, CellAccessOptions options, long cellId, Triple TripleCell = default(Triple))
        {
            
            byte* targetPtr;
            
            targetPtr = null;
            
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            targetPtr += strlen_3+sizeof(int);
        }else
        {
            targetPtr += sizeof(int);
        }

            }
            }
            byte[] tmpcell = new byte[(int)(targetPtr)];
            fixed (byte* _tmpcellptr = tmpcell)
            {
                targetPtr = _tmpcellptr;
                
            {

            {

        if(TripleCell.Subject!= null)
        {
            int strlen_3 = TripleCell.Subject.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Subject)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Predicate!= null)
        {
            int strlen_3 = TripleCell.Predicate.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Predicate)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Object!= null)
        {
            int strlen_3 = TripleCell.Object.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Object)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

        if(TripleCell.Namespace!= null)
        {
            int strlen_3 = TripleCell.Namespace.Length * 2;
            *(int*)targetPtr = strlen_3;
            targetPtr += sizeof(int);
            fixed(char* pstr_3 = TripleCell.Namespace)
            {
                Memory.Copy(pstr_3, targetPtr, strlen_3);
                targetPtr += strlen_3;
            }
        }else
        {
            *(int*)targetPtr = 0;
            targetPtr += sizeof(int);
        }

            }
            }
            }
            
            return storage.SaveCell(tx, options, cellId, tmpcell, 0, tmpcell.Length, (ushort)CellType.TripleStore) == TrinityErrorCode.E_SUCCESS;
        }
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. The parameter <paramref name="cellId"/> overrides the cell id in the content object.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="cellId">A 64-bit cell Id.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <param name="cellContent">The content of the cell.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx, CellAccessOptions options, long cellId, TripleStore cellContent)
        {
            return SaveTripleStore(storage, tx, options, cellId  , cellContent.TripleCell );
        }
        /// <summary>
        /// Adds a new cell of type TripleStore to the key-value store if the cell Id does not exist, or updates an existing cell in the key-value store if the cell Id already exists. Cell Id is specified by the CellId field in the content object.
        /// </summary>
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// <param name="options">Specifies write-ahead logging behavior. Valid values are CellAccessOptions.StrongLogAhead(default) and CellAccessOptions.WeakLogAhead. Other values are ignored.</param>
        /// <param name="cellContent">The content of the cell.</param>
        /// <returns>true if saving succeeds; otherwise, false.</returns>
        public unsafe static bool SaveTripleStore(this Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx, CellAccessOptions options, TripleStore cellContent)
        {
            return SaveTripleStore(storage, tx, options, cellContent.CellId  , cellContent.TripleCell );
        }
        /// <summary>
        /// Loads the content of the specified cell. Any changes done to this object are not written to the store, unless
        /// the content object is saved back into the storage.
        /// <param name="storage"/>A <see cref="Trinity.Storage.LocalMemoryStorage"/> instance.</param>
        /// </summary>
        public unsafe static TripleStore LoadTripleStore(this Trinity.Storage.LocalMemoryStorage storage, LocalTransactionContext tx, long cellId)
        {
            using (var cell = TripleStore_Accessor._get()._Lock(cellId, CellAccessOptions.ThrowExceptionOnCellNotFound, tx))
            {
                return cell;
            }
        }
        #endregion
    }
}

#pragma warning restore 162,168,649,660,661,1522

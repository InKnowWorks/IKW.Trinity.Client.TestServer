#pragma warning disable 162,168,649,660,661,1522
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity.Storage;
using Trinity.TSL;
namespace Trinity.Client.TestProtocols
{
    /// <summary>
    /// Exposes the data modeling schema defined in the TSL.
    /// </summary>
    public class StorageSchema : IStorageSchema
    {
        #region CellType lookup table
        internal static Dictionary<string, CellType> cellTypeLookupTable = new Dictionary<string, CellType>()
        {
            
            {"TripleStore", global::Trinity.Client.TestProtocols.CellType.TripleStore}
            
        };
        #endregion
        
        internal static readonly Type   s_cellType_TripleStore       = typeof(global::Trinity.Client.TestProtocols.TripleStore);
        internal static readonly string s_cellTypeName_TripleStore   = "TripleStore";
        internal class TripleStore_descriptor : ICellDescriptor
        {
            private static IReadOnlyDictionary<string, string> s_attributes = new Dictionary<string, string>
            {
                
            };
            internal static bool check_attribute(IAttributeCollection attributes, string attributeKey, string attributeValue)
            {
                if (attributeKey == null)
                    return true;
                if (attributeValue == null)
                    return attributes.Attributes.ContainsKey(attributeKey);
                else
                    return attributes.Attributes.ContainsKey(attributeKey) && attributes.Attributes[attributeKey] == attributeValue;
            }
            
            internal class TripleCell_descriptor : IFieldDescriptor
            {
                private static IReadOnlyDictionary<string, string> s_attributes = new Dictionary<string, string>
                {
                    
                };
                private static string s_typename = "Triple";
                private static Type   s_type     = typeof(Triple);
                public string Name
                {
                    get { return "TripleCell"; }
                }
                public bool Optional
                {
                    get
                    {
                        
                        return false;
                        
                    }
                }
                public bool IsOfType<T>()
                {
                    return typeof(T) == Type;
                }
                public bool IsList()
                {
                    
                    return false;
                    
                }
                public string TypeName
                {
                    get { return s_typename; }
                }
                public Type Type
                {
                    get { return s_type; }
                }
                public IReadOnlyDictionary<string, string> Attributes
                {
                    get { return s_attributes; }
                }
                public string GetAttributeValue(string attributeKey)
                {
                    string ret = null;
                    s_attributes.TryGetValue(attributeKey, out ret);
                    return ret;
                }
            }
            internal static TripleCell_descriptor TripleCell = new TripleCell_descriptor();
            
            #region ICellDescriptor
            public IEnumerable<string> GetFieldNames()
            {
                
                yield return "TripleCell";
                
            }
            public IAttributeCollection GetFieldAttributes(string fieldName)
            {
                int field_id = global::Trinity.Client.TestProtocols.TripleStore.FieldLookupTable.Lookup(fieldName);
                if (field_id == -1)
                    Throw.undefined_field();
                switch (field_id)
                {
                    
                    case 0:
                        return TripleCell;
                    
                }
                /* Should not reach here */
                throw new Exception("Internal error T6001");
            }
            public IEnumerable<IFieldDescriptor> GetFieldDescriptors()
            {
                
                yield return TripleCell;
                
            }
            ushort ICellDescriptor.CellType
            {
                get { return (ushort)CellType.TripleStore; }
            }
            #endregion
            #region ITypeDescriptor
            public string TypeName
            {
                get { return s_cellTypeName_TripleStore; }
            }
            public Type Type
            {
                get { return s_cellType_TripleStore; }
            }
            public bool IsOfType<T>()
            {
                return typeof(T) == s_cellType_TripleStore;
            }
            public bool IsList()
            {
                return false;
            }
            #endregion
            #region IAttributeCollection
            public IReadOnlyDictionary<string, string> Attributes
            {
                get { return s_attributes; }
            }
            public string GetAttributeValue(string attributeKey)
            {
                string ret = null;
                s_attributes.TryGetValue(attributeKey, out ret);
                return ret;
            }
            #endregion
        }
        internal static readonly TripleStore_descriptor s_cellDescriptor_TripleStore = new TripleStore_descriptor();
        /// <summary>
        /// Get the cell descriptor for TripleStore.
        /// </summary>
        public static ICellDescriptor TripleStore { get { return s_cellDescriptor_TripleStore; } }
        
        /// <summary>
        /// Enumerates descriptors for all cells defined in the TSL.
        /// </summary>
        public static IEnumerable<ICellDescriptor> CellDescriptors
        {
            get
            {
                
                yield return TripleStore;
                
                yield break;
            }
        }
        /// <summary>
        /// Converts a type string to <see cref="Trinity.Client.TestProtocols.CellType"/>.
        /// </summary>
        /// <param name="cellTypeString">The type string to be converted.</param>
        /// <returns>The converted cell type.</returns>
        public static CellType GetCellType(string cellTypeString)
        {
            CellType ret;
            if (!cellTypeLookupTable.TryGetValue(cellTypeString, out ret))
                throw new Exception("Unrecognized cell type string.");
            return ret;
        }
        #region IStorageSchema implementation
        IEnumerable<ICellDescriptor> IStorageSchema.CellDescriptors
        {
            get { return StorageSchema.CellDescriptors; }
        }
        ushort IStorageSchema.GetCellType(string cellTypeString)
        {
            return (ushort)StorageSchema.GetCellType(cellTypeString);
        }
        IEnumerable<string> IStorageSchema.CellTypeSignatures
        {
            get
            {
                
                yield return "{{string|string|string|string}}";
                
                yield break;
            }
        }
        #endregion
    }
}

#pragma warning restore 162,168,649,660,661,1522

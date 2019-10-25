#pragma warning disable 162,168,649,660,661,1522

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trinity.TSL;
using Trinity.TSL.Lib;
namespace Trinity.Client.TestProtocols
{
    internal class TypeSystem
    {
        #region TypeID lookup table
        private static Dictionary<Type, uint> TypeIDLookupTable = new Dictionary<Type, uint>()
        {
            
            { typeof(int), 0 }
            ,
            { typeof(string), 1 }
            ,
            { typeof(List<string>), 2 }
            ,
            { typeof(List<Triple>), 3 }
            ,
            { typeof(Triple), 4 }
            ,
        };
        #endregion
        #region CellTypeID lookup table
        private static Dictionary<Type, uint> CellTypeIDLookupTable = new Dictionary<Type, uint>()
        {
            
            { typeof(TripleStore), 0 }
            
        };
        #endregion
        internal static uint GetTypeID(Type t)
        {
            uint type_id;
            if (!TypeIDLookupTable.TryGetValue(t, out type_id))
                type_id = uint.MaxValue;
            return type_id;
        }
        internal static uint GetCellTypeID(Type t)
        {
            uint type_id;
            if (!CellTypeIDLookupTable.TryGetValue(t, out type_id))
                throw new Exception("Type " + t.ToString() + " is not a cell.");
            return type_id;
        }
    }
    internal enum TypeConversionAction
    {
        TC_NONCONVERTIBLE = 0,
        TC_ASSIGN,
        TC_TOSTRING,
        TC_PARSESTRING,
        TC_TOBOOL,
        TC_CONVERTLIST,
        TC_WRAPINLIST,
        TC_ARRAYTOLIST,
        TC_EXTRACTNULLABLE,
    }
    internal interface ITypeConverter<T>
    {
        
        T ConvertFrom_int(int value);
        int ConvertTo_int(T value);
        TypeConversionAction GetConversionActionTo_int();
        IEnumerable<int> Enumerate_int(T value);
        
        T ConvertFrom_string(string value);
        string ConvertTo_string(T value);
        TypeConversionAction GetConversionActionTo_string();
        IEnumerable<string> Enumerate_string(T value);
        
        T ConvertFrom_List_string(List<string> value);
        List<string> ConvertTo_List_string(T value);
        TypeConversionAction GetConversionActionTo_List_string();
        IEnumerable<List<string>> Enumerate_List_string(T value);
        
        T ConvertFrom_List_Triple(List<Triple> value);
        List<Triple> ConvertTo_List_Triple(T value);
        TypeConversionAction GetConversionActionTo_List_Triple();
        IEnumerable<List<Triple>> Enumerate_List_Triple(T value);
        
        T ConvertFrom_Triple(Triple value);
        Triple ConvertTo_Triple(T value);
        TypeConversionAction GetConversionActionTo_Triple();
        IEnumerable<Triple> Enumerate_Triple(T value);
        
    }
    internal class TypeConverter<T> : ITypeConverter<T>
    {
        internal class _TypeConverterImpl : ITypeConverter<object>
            
            , ITypeConverter<int>
        
            , ITypeConverter<string>
        
            , ITypeConverter<List<string>>
        
            , ITypeConverter<List<Triple>>
        
            , ITypeConverter<Triple>
        
        {
            int ITypeConverter<int>.ConvertFrom_int(int value)
            {
                
                return (int)value;
                
            }
            int ITypeConverter<int>.ConvertTo_int(int value)
            {
                return TypeConverter<int>.ConvertFrom_int(value);
            }
            TypeConversionAction ITypeConverter<int>.GetConversionActionTo_int()
            {
                
                return TypeConversionAction.TC_ASSIGN;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<int> ITypeConverter<int>.Enumerate_int(int value)
            {
                
                yield break;
            }
            int ITypeConverter<int>.ConvertFrom_string(string value)
            {
                
                {
                    #region String parse
                    int intermediate_result;
                    bool conversion_success;
                    
                    {
                        conversion_success = int.TryParse(value, out intermediate_result);
                    }
                    
                    if (!conversion_success)
                    {
                        
                        Throw.cannot_parse(value, "int");
                        
                    }
                    return intermediate_result;
                    #endregion
                }
                
            }
            string ITypeConverter<int>.ConvertTo_string(int value)
            {
                return TypeConverter<string>.ConvertFrom_int(value);
            }
            TypeConversionAction ITypeConverter<int>.GetConversionActionTo_string()
            {
                
                return TypeConversionAction.TC_TOSTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<string> ITypeConverter<int>.Enumerate_string(int value)
            {
                
                yield break;
            }
            int ITypeConverter<int>.ConvertFrom_List_string(List<string> value)
            {
                
                throw new InvalidCastException("Invalid cast from 'List<string>' to 'int'.");
                
            }
            List<string> ITypeConverter<int>.ConvertTo_List_string(int value)
            {
                return TypeConverter<List<string>>.ConvertFrom_int(value);
            }
            TypeConversionAction ITypeConverter<int>.GetConversionActionTo_List_string()
            {
                
                return TypeConversionAction.TC_WRAPINLIST;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<string>> ITypeConverter<int>.Enumerate_List_string(int value)
            {
                
                yield break;
            }
            int ITypeConverter<int>.ConvertFrom_List_Triple(List<Triple> value)
            {
                
                throw new InvalidCastException("Invalid cast from 'List<Triple>' to 'int'.");
                
            }
            List<Triple> ITypeConverter<int>.ConvertTo_List_Triple(int value)
            {
                return TypeConverter<List<Triple>>.ConvertFrom_int(value);
            }
            TypeConversionAction ITypeConverter<int>.GetConversionActionTo_List_Triple()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<Triple>> ITypeConverter<int>.Enumerate_List_Triple(int value)
            {
                
                yield break;
            }
            int ITypeConverter<int>.ConvertFrom_Triple(Triple value)
            {
                
                throw new InvalidCastException("Invalid cast from 'Triple' to 'int'.");
                
            }
            Triple ITypeConverter<int>.ConvertTo_Triple(int value)
            {
                return TypeConverter<Triple>.ConvertFrom_int(value);
            }
            TypeConversionAction ITypeConverter<int>.GetConversionActionTo_Triple()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<Triple> ITypeConverter<int>.Enumerate_Triple(int value)
            {
                
                yield break;
            }
            string ITypeConverter<string>.ConvertFrom_int(int value)
            {
                
                return Serializer.ToString(value);
                
            }
            int ITypeConverter<string>.ConvertTo_int(string value)
            {
                return TypeConverter<int>.ConvertFrom_string(value);
            }
            TypeConversionAction ITypeConverter<string>.GetConversionActionTo_int()
            {
                
                return TypeConversionAction.TC_PARSESTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<int> ITypeConverter<string>.Enumerate_int(string value)
            {
                
                yield break;
            }
            string ITypeConverter<string>.ConvertFrom_string(string value)
            {
                
                return (string)value;
                
            }
            string ITypeConverter<string>.ConvertTo_string(string value)
            {
                return TypeConverter<string>.ConvertFrom_string(value);
            }
            TypeConversionAction ITypeConverter<string>.GetConversionActionTo_string()
            {
                
                return TypeConversionAction.TC_ASSIGN;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<string> ITypeConverter<string>.Enumerate_string(string value)
            {
                
                yield break;
            }
            string ITypeConverter<string>.ConvertFrom_List_string(List<string> value)
            {
                
                return Serializer.ToString(value);
                
            }
            List<string> ITypeConverter<string>.ConvertTo_List_string(string value)
            {
                return TypeConverter<List<string>>.ConvertFrom_string(value);
            }
            TypeConversionAction ITypeConverter<string>.GetConversionActionTo_List_string()
            {
                
                return TypeConversionAction.TC_PARSESTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<string>> ITypeConverter<string>.Enumerate_List_string(string value)
            {
                
                yield break;
            }
            string ITypeConverter<string>.ConvertFrom_List_Triple(List<Triple> value)
            {
                
                return Serializer.ToString(value);
                
            }
            List<Triple> ITypeConverter<string>.ConvertTo_List_Triple(string value)
            {
                return TypeConverter<List<Triple>>.ConvertFrom_string(value);
            }
            TypeConversionAction ITypeConverter<string>.GetConversionActionTo_List_Triple()
            {
                
                return TypeConversionAction.TC_PARSESTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<Triple>> ITypeConverter<string>.Enumerate_List_Triple(string value)
            {
                
                yield break;
            }
            string ITypeConverter<string>.ConvertFrom_Triple(Triple value)
            {
                
                return Serializer.ToString(value);
                
            }
            Triple ITypeConverter<string>.ConvertTo_Triple(string value)
            {
                return TypeConverter<Triple>.ConvertFrom_string(value);
            }
            TypeConversionAction ITypeConverter<string>.GetConversionActionTo_Triple()
            {
                
                return TypeConversionAction.TC_PARSESTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<Triple> ITypeConverter<string>.Enumerate_Triple(string value)
            {
                
                yield break;
            }
            List<string> ITypeConverter<List<string>>.ConvertFrom_int(int value)
            {
                
                {
                    List<string> intermediate_result = new List<string>();
                    intermediate_result.Add(TypeConverter<string>.ConvertFrom_int(value));
                    return intermediate_result;
                }
                
            }
            int ITypeConverter<List<string>>.ConvertTo_int(List<string> value)
            {
                return TypeConverter<int>.ConvertFrom_List_string(value);
            }
            TypeConversionAction ITypeConverter<List<string>>.GetConversionActionTo_int()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<int> ITypeConverter<List<string>>.Enumerate_int(List<string> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<int>.ConvertFrom_string(element);
                
                yield break;
            }
            List<string> ITypeConverter<List<string>>.ConvertFrom_string(string value)
            {
                
                {
                    #region String parse
                    List<string> intermediate_result;
                    bool conversion_success;
                    
                    {
                        conversion_success = ExternalParser.TryParse_List_string(value, out intermediate_result);
                    }
                    
                    if (!conversion_success)
                    {
                        
                        try
                        {
                            string element = TypeConverter<string>.ConvertFrom_string(value);
                            intermediate_result = new List<string>();
                            intermediate_result.Add(element);
                        }
                        catch
                        {
                            throw new ArgumentException("Cannot parse \"" + value + "\" into either 'List<string>' or 'string'.");
                        }
                        
                    }
                    return intermediate_result;
                    #endregion
                }
                
            }
            string ITypeConverter<List<string>>.ConvertTo_string(List<string> value)
            {
                return TypeConverter<string>.ConvertFrom_List_string(value);
            }
            TypeConversionAction ITypeConverter<List<string>>.GetConversionActionTo_string()
            {
                
                return TypeConversionAction.TC_TOSTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<string> ITypeConverter<List<string>>.Enumerate_string(List<string> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<string>.ConvertFrom_string(element);
                
                yield break;
            }
            List<string> ITypeConverter<List<string>>.ConvertFrom_List_string(List<string> value)
            {
                
                return (List<string>)value;
                
            }
            List<string> ITypeConverter<List<string>>.ConvertTo_List_string(List<string> value)
            {
                return TypeConverter<List<string>>.ConvertFrom_List_string(value);
            }
            TypeConversionAction ITypeConverter<List<string>>.GetConversionActionTo_List_string()
            {
                
                return TypeConversionAction.TC_ASSIGN;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<string>> ITypeConverter<List<string>>.Enumerate_List_string(List<string> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<List<string>>.ConvertFrom_string(element);
                
                yield break;
            }
            List<string> ITypeConverter<List<string>>.ConvertFrom_List_Triple(List<Triple> value)
            {
                
                {
                    List<string> intermediate_result = new List<string>();
                    foreach (var element in value)
                    {
                        intermediate_result.Add(TypeConverter<string>.ConvertFrom_Triple(element));
                    }
                    return intermediate_result;
                }
                
            }
            List<Triple> ITypeConverter<List<string>>.ConvertTo_List_Triple(List<string> value)
            {
                return TypeConverter<List<Triple>>.ConvertFrom_List_string(value);
            }
            TypeConversionAction ITypeConverter<List<string>>.GetConversionActionTo_List_Triple()
            {
                
                return TypeConversionAction.TC_CONVERTLIST;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<Triple>> ITypeConverter<List<string>>.Enumerate_List_Triple(List<string> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<List<Triple>>.ConvertFrom_string(element);
                
                yield break;
            }
            List<string> ITypeConverter<List<string>>.ConvertFrom_Triple(Triple value)
            {
                
                {
                    List<string> intermediate_result = new List<string>();
                    intermediate_result.Add(TypeConverter<string>.ConvertFrom_Triple(value));
                    return intermediate_result;
                }
                
            }
            Triple ITypeConverter<List<string>>.ConvertTo_Triple(List<string> value)
            {
                return TypeConverter<Triple>.ConvertFrom_List_string(value);
            }
            TypeConversionAction ITypeConverter<List<string>>.GetConversionActionTo_Triple()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<Triple> ITypeConverter<List<string>>.Enumerate_Triple(List<string> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<Triple>.ConvertFrom_string(element);
                
                yield break;
            }
            List<Triple> ITypeConverter<List<Triple>>.ConvertFrom_int(int value)
            {
                
                throw new InvalidCastException("Invalid cast from 'int' to 'List<Triple>'.");
                
            }
            int ITypeConverter<List<Triple>>.ConvertTo_int(List<Triple> value)
            {
                return TypeConverter<int>.ConvertFrom_List_Triple(value);
            }
            TypeConversionAction ITypeConverter<List<Triple>>.GetConversionActionTo_int()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<int> ITypeConverter<List<Triple>>.Enumerate_int(List<Triple> value)
            {
                
                yield break;
            }
            List<Triple> ITypeConverter<List<Triple>>.ConvertFrom_string(string value)
            {
                
                {
                    #region String parse
                    List<Triple> intermediate_result;
                    bool conversion_success;
                    
                    {
                        conversion_success = ExternalParser.TryParse_List_Triple(value, out intermediate_result);
                    }
                    
                    if (!conversion_success)
                    {
                        
                        try
                        {
                            Triple element = TypeConverter<Triple>.ConvertFrom_string(value);
                            intermediate_result = new List<Triple>();
                            intermediate_result.Add(element);
                        }
                        catch
                        {
                            throw new ArgumentException("Cannot parse \"" + value + "\" into either 'List<Triple>' or 'Triple'.");
                        }
                        
                    }
                    return intermediate_result;
                    #endregion
                }
                
            }
            string ITypeConverter<List<Triple>>.ConvertTo_string(List<Triple> value)
            {
                return TypeConverter<string>.ConvertFrom_List_Triple(value);
            }
            TypeConversionAction ITypeConverter<List<Triple>>.GetConversionActionTo_string()
            {
                
                return TypeConversionAction.TC_TOSTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<string> ITypeConverter<List<Triple>>.Enumerate_string(List<Triple> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<string>.ConvertFrom_Triple(element);
                
                yield break;
            }
            List<Triple> ITypeConverter<List<Triple>>.ConvertFrom_List_string(List<string> value)
            {
                
                {
                    List<Triple> intermediate_result = new List<Triple>();
                    foreach (var element in value)
                    {
                        intermediate_result.Add(TypeConverter<Triple>.ConvertFrom_string(element));
                    }
                    return intermediate_result;
                }
                
            }
            List<string> ITypeConverter<List<Triple>>.ConvertTo_List_string(List<Triple> value)
            {
                return TypeConverter<List<string>>.ConvertFrom_List_Triple(value);
            }
            TypeConversionAction ITypeConverter<List<Triple>>.GetConversionActionTo_List_string()
            {
                
                return TypeConversionAction.TC_CONVERTLIST;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<string>> ITypeConverter<List<Triple>>.Enumerate_List_string(List<Triple> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<List<string>>.ConvertFrom_Triple(element);
                
                yield break;
            }
            List<Triple> ITypeConverter<List<Triple>>.ConvertFrom_List_Triple(List<Triple> value)
            {
                
                return (List<Triple>)value;
                
            }
            List<Triple> ITypeConverter<List<Triple>>.ConvertTo_List_Triple(List<Triple> value)
            {
                return TypeConverter<List<Triple>>.ConvertFrom_List_Triple(value);
            }
            TypeConversionAction ITypeConverter<List<Triple>>.GetConversionActionTo_List_Triple()
            {
                
                return TypeConversionAction.TC_ASSIGN;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<Triple>> ITypeConverter<List<Triple>>.Enumerate_List_Triple(List<Triple> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<List<Triple>>.ConvertFrom_Triple(element);
                
                yield break;
            }
            List<Triple> ITypeConverter<List<Triple>>.ConvertFrom_Triple(Triple value)
            {
                
                {
                    List<Triple> intermediate_result = new List<Triple>();
                    intermediate_result.Add(TypeConverter<Triple>.ConvertFrom_Triple(value));
                    return intermediate_result;
                }
                
            }
            Triple ITypeConverter<List<Triple>>.ConvertTo_Triple(List<Triple> value)
            {
                return TypeConverter<Triple>.ConvertFrom_List_Triple(value);
            }
            TypeConversionAction ITypeConverter<List<Triple>>.GetConversionActionTo_Triple()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<Triple> ITypeConverter<List<Triple>>.Enumerate_Triple(List<Triple> value)
            {
                
                foreach (var element in value)
                    yield return TypeConverter<Triple>.ConvertFrom_Triple(element);
                
                yield break;
            }
            Triple ITypeConverter<Triple>.ConvertFrom_int(int value)
            {
                
                throw new InvalidCastException("Invalid cast from 'int' to 'Triple'.");
                
            }
            int ITypeConverter<Triple>.ConvertTo_int(Triple value)
            {
                return TypeConverter<int>.ConvertFrom_Triple(value);
            }
            TypeConversionAction ITypeConverter<Triple>.GetConversionActionTo_int()
            {
                
                return TypeConversionAction.TC_NONCONVERTIBLE;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<int> ITypeConverter<Triple>.Enumerate_int(Triple value)
            {
                
                yield break;
            }
            Triple ITypeConverter<Triple>.ConvertFrom_string(string value)
            {
                
                {
                    #region String parse
                    Triple intermediate_result;
                    bool conversion_success;
                    
                    {
                        conversion_success = Triple.TryParse(value, out intermediate_result);
                    }
                    
                    if (!conversion_success)
                    {
                        
                        Throw.cannot_parse(value, "Triple");
                        
                    }
                    return intermediate_result;
                    #endregion
                }
                
            }
            string ITypeConverter<Triple>.ConvertTo_string(Triple value)
            {
                return TypeConverter<string>.ConvertFrom_Triple(value);
            }
            TypeConversionAction ITypeConverter<Triple>.GetConversionActionTo_string()
            {
                
                return TypeConversionAction.TC_TOSTRING;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<string> ITypeConverter<Triple>.Enumerate_string(Triple value)
            {
                
                yield break;
            }
            Triple ITypeConverter<Triple>.ConvertFrom_List_string(List<string> value)
            {
                
                throw new InvalidCastException("Invalid cast from 'List<string>' to 'Triple'.");
                
            }
            List<string> ITypeConverter<Triple>.ConvertTo_List_string(Triple value)
            {
                return TypeConverter<List<string>>.ConvertFrom_Triple(value);
            }
            TypeConversionAction ITypeConverter<Triple>.GetConversionActionTo_List_string()
            {
                
                return TypeConversionAction.TC_WRAPINLIST;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<string>> ITypeConverter<Triple>.Enumerate_List_string(Triple value)
            {
                
                yield break;
            }
            Triple ITypeConverter<Triple>.ConvertFrom_List_Triple(List<Triple> value)
            {
                
                throw new InvalidCastException("Invalid cast from 'List<Triple>' to 'Triple'.");
                
            }
            List<Triple> ITypeConverter<Triple>.ConvertTo_List_Triple(Triple value)
            {
                return TypeConverter<List<Triple>>.ConvertFrom_Triple(value);
            }
            TypeConversionAction ITypeConverter<Triple>.GetConversionActionTo_List_Triple()
            {
                
                return TypeConversionAction.TC_WRAPINLIST;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<List<Triple>> ITypeConverter<Triple>.Enumerate_List_Triple(Triple value)
            {
                
                yield break;
            }
            Triple ITypeConverter<Triple>.ConvertFrom_Triple(Triple value)
            {
                
                return (Triple)value;
                
            }
            Triple ITypeConverter<Triple>.ConvertTo_Triple(Triple value)
            {
                return TypeConverter<Triple>.ConvertFrom_Triple(value);
            }
            TypeConversionAction ITypeConverter<Triple>.GetConversionActionTo_Triple()
            {
                
                return TypeConversionAction.TC_ASSIGN;
                
            }
            /// <summary>
            /// ONLY VALID FOR TC_CONVERTLIST AND TC_ARRAYTOLIST.
            /// </summary>
            IEnumerable<Triple> ITypeConverter<Triple>.Enumerate_Triple(Triple value)
            {
                
                yield break;
            }
            
            object ITypeConverter<object>.ConvertFrom_int(int value)
            {
                return value;
            }
            int ITypeConverter<object>.ConvertTo_int(object value)
            {
                throw new NotImplementedException();
            }
            TypeConversionAction ITypeConverter<object>.GetConversionActionTo_int()
            {
                throw new NotImplementedException();
            }
            IEnumerable<int> ITypeConverter<object>.Enumerate_int(object value)
            {
                throw new NotImplementedException();
            }
            
            object ITypeConverter<object>.ConvertFrom_string(string value)
            {
                return value;
            }
            string ITypeConverter<object>.ConvertTo_string(object value)
            {
                throw new NotImplementedException();
            }
            TypeConversionAction ITypeConverter<object>.GetConversionActionTo_string()
            {
                throw new NotImplementedException();
            }
            IEnumerable<string> ITypeConverter<object>.Enumerate_string(object value)
            {
                throw new NotImplementedException();
            }
            
            object ITypeConverter<object>.ConvertFrom_List_string(List<string> value)
            {
                return value;
            }
            List<string> ITypeConverter<object>.ConvertTo_List_string(object value)
            {
                throw new NotImplementedException();
            }
            TypeConversionAction ITypeConverter<object>.GetConversionActionTo_List_string()
            {
                throw new NotImplementedException();
            }
            IEnumerable<List<string>> ITypeConverter<object>.Enumerate_List_string(object value)
            {
                throw new NotImplementedException();
            }
            
            object ITypeConverter<object>.ConvertFrom_List_Triple(List<Triple> value)
            {
                return value;
            }
            List<Triple> ITypeConverter<object>.ConvertTo_List_Triple(object value)
            {
                throw new NotImplementedException();
            }
            TypeConversionAction ITypeConverter<object>.GetConversionActionTo_List_Triple()
            {
                throw new NotImplementedException();
            }
            IEnumerable<List<Triple>> ITypeConverter<object>.Enumerate_List_Triple(object value)
            {
                throw new NotImplementedException();
            }
            
            object ITypeConverter<object>.ConvertFrom_Triple(Triple value)
            {
                return value;
            }
            Triple ITypeConverter<object>.ConvertTo_Triple(object value)
            {
                throw new NotImplementedException();
            }
            TypeConversionAction ITypeConverter<object>.GetConversionActionTo_Triple()
            {
                throw new NotImplementedException();
            }
            IEnumerable<Triple> ITypeConverter<object>.Enumerate_Triple(object value)
            {
                throw new NotImplementedException();
            }
            
        }
        internal static readonly ITypeConverter<T> s_type_converter = new _TypeConverterImpl() as ITypeConverter<T> ?? new TypeConverter<T>();
        #region Default implementation
        
        T ITypeConverter<T>.ConvertFrom_int(int value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        int ITypeConverter<T>.ConvertTo_int(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        TypeConversionAction ITypeConverter<T>.GetConversionActionTo_int()
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        IEnumerable<int> ITypeConverter<T>.Enumerate_int(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        
        T ITypeConverter<T>.ConvertFrom_string(string value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        string ITypeConverter<T>.ConvertTo_string(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        TypeConversionAction ITypeConverter<T>.GetConversionActionTo_string()
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        IEnumerable<string> ITypeConverter<T>.Enumerate_string(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        
        T ITypeConverter<T>.ConvertFrom_List_string(List<string> value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        List<string> ITypeConverter<T>.ConvertTo_List_string(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        TypeConversionAction ITypeConverter<T>.GetConversionActionTo_List_string()
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        IEnumerable<List<string>> ITypeConverter<T>.Enumerate_List_string(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        
        T ITypeConverter<T>.ConvertFrom_List_Triple(List<Triple> value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        List<Triple> ITypeConverter<T>.ConvertTo_List_Triple(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        TypeConversionAction ITypeConverter<T>.GetConversionActionTo_List_Triple()
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        IEnumerable<List<Triple>> ITypeConverter<T>.Enumerate_List_Triple(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        
        T ITypeConverter<T>.ConvertFrom_Triple(Triple value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        Triple ITypeConverter<T>.ConvertTo_Triple(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        TypeConversionAction ITypeConverter<T>.GetConversionActionTo_Triple()
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        IEnumerable<Triple> ITypeConverter<T>.Enumerate_Triple(T value)
        {
            throw new NotImplementedException("Internal error T5013.");
        }
        
        #endregion
        internal static readonly uint type_id = TypeSystem.GetTypeID(typeof(T));
        
        internal static T ConvertFrom_int(int value)
        {
            return s_type_converter.ConvertFrom_int(value);
        }
        internal static int ConvertTo_int(T value)
        {
            return s_type_converter.ConvertTo_int(value);
        }
        internal static TypeConversionAction GetConversionActionTo_int()
        {
            return s_type_converter.GetConversionActionTo_int();
        }
        internal static IEnumerable<int> Enumerate_int(T value)
        {
            return s_type_converter.Enumerate_int(value);
        }
        
        internal static T ConvertFrom_string(string value)
        {
            return s_type_converter.ConvertFrom_string(value);
        }
        internal static string ConvertTo_string(T value)
        {
            return s_type_converter.ConvertTo_string(value);
        }
        internal static TypeConversionAction GetConversionActionTo_string()
        {
            return s_type_converter.GetConversionActionTo_string();
        }
        internal static IEnumerable<string> Enumerate_string(T value)
        {
            return s_type_converter.Enumerate_string(value);
        }
        
        internal static T ConvertFrom_List_string(List<string> value)
        {
            return s_type_converter.ConvertFrom_List_string(value);
        }
        internal static List<string> ConvertTo_List_string(T value)
        {
            return s_type_converter.ConvertTo_List_string(value);
        }
        internal static TypeConversionAction GetConversionActionTo_List_string()
        {
            return s_type_converter.GetConversionActionTo_List_string();
        }
        internal static IEnumerable<List<string>> Enumerate_List_string(T value)
        {
            return s_type_converter.Enumerate_List_string(value);
        }
        
        internal static T ConvertFrom_List_Triple(List<Triple> value)
        {
            return s_type_converter.ConvertFrom_List_Triple(value);
        }
        internal static List<Triple> ConvertTo_List_Triple(T value)
        {
            return s_type_converter.ConvertTo_List_Triple(value);
        }
        internal static TypeConversionAction GetConversionActionTo_List_Triple()
        {
            return s_type_converter.GetConversionActionTo_List_Triple();
        }
        internal static IEnumerable<List<Triple>> Enumerate_List_Triple(T value)
        {
            return s_type_converter.Enumerate_List_Triple(value);
        }
        
        internal static T ConvertFrom_Triple(Triple value)
        {
            return s_type_converter.ConvertFrom_Triple(value);
        }
        internal static Triple ConvertTo_Triple(T value)
        {
            return s_type_converter.ConvertTo_Triple(value);
        }
        internal static TypeConversionAction GetConversionActionTo_Triple()
        {
            return s_type_converter.GetConversionActionTo_Triple();
        }
        internal static IEnumerable<Triple> Enumerate_Triple(T value)
        {
            return s_type_converter.Enumerate_Triple(value);
        }
        
    }
}

#pragma warning restore 162,168,649,660,661,1522

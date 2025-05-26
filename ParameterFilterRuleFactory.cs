#region Assembly RevitAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files\Autodesk\Revit 2023\RevitAPI.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.Proxy.DB;
using Autodesk.Revit.Proxy.Exceptions;

namespace Autodesk.Revit.DB;

//
// Summary:
//     Contains functions that create appropriate FilterRule objects based on the parameters
//     given.
public class ParameterFilterRuleFactory : IDisposable
{
    internal ParameterFilterRuleFactoryProxy m_proxy = (ParameterFilterRuleFactoryProxy)((proxy is ParameterFilterRuleFactoryProxy) ? proxy : null);

    //
    // Summary:
    //     Specifies whether the .NET object represents a valid Revit entity.
    //
    // Returns:
    //     True if the API object holds a valid Revit native object, false otherwise.
    //
    // Remarks:
    //     If the corresponding Revit native object is destroyed, or creation of the corresponding
    //     object is undone, a managed API object containing it is no longer valid. API
    //     methods cannot be called on invalidated wrapper objects.
    public bool IsValidObject
    {
        [return: MarshalAs(UnmanagedType.U1)]
        get
        {
            return m_proxy.IsValidObject;
        }
    }

    internal ParameterFilterRuleFactory(object proxy)
    {
    }

    private void _007EParameterFilterRuleFactory()
    {
        ReleaseUnmanagedResources(disposing: true);
    }

    private void _0021ParameterFilterRuleFactory()
    {
        ReleaseUnmanagedResources(disposing: false);
    }

    protected virtual void ReleaseUnmanagedResources([MarshalAs(UnmanagedType.U1)] bool disposing)
    {
        if (disposing)
        {
            ((IDisposable)m_proxy)?.Dispose();
        }
    }

    //
    // Summary:
    //     Creates a filter rule that tests elements for support of a shared parameter.
    //
    //
    // Parameters:
    //   parameterName:
    //     The name of the parameter that elements must support to satisfy this rule.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateSharedParameterApplicableRule(string parameterName)
    {
        //IL_000a: Expected I8, but got I
        //IL_0030: Expected O, but got Unknown
        //IL_00c2: Expected I, but got I8
        //IL_0075: Expected I, but got I8
        //IL_0075: Expected I, but got I8
        //IL_0085: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            string text = null;
            text = parameterName;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateSharedParameterApplicableRule(parameterName);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 255, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateSharedParameterApplicableRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 255, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateSharedParameterApplicableRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether ElementId values from the document
    //     equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     An ElementId-typed parameter used to get values from the document for a given
    //     element.
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateEqualsRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEqualsRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 344, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 344, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether integer values from the document
    //     equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     An integer-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateEqualsRule(ElementId parameter, int value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004b: Expected O, but got Unknown
        //IL_00dd: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_00a0: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEqualsRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 327, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 327, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether double-precision values from the
    //     document equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     A double-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    //   epsilon:
    //     Defines the tolerance within which two values may be considered equal.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentException:
    //     The given value for value is not finite -or- The given value for value is not
    //     a number -or- The given value for epsilon is not finite -or- The given value
    //     for epsilon is not a number
    //
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateEqualsRule(ElementId parameter, double value, double epsilon)
    {
        //IL_000b: Expected I8, but got I
        //IL_004f: Expected O, but got Unknown
        //IL_00e1: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_00a4: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEqualsRule(val, value, epsilon);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 310, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 310, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document equal
    //     a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateEqualsRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEqualsRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 291, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 291, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document equal
    //     a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateEqualsRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEqualsRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 274, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 274, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether ElementId values from the document
    //     do not equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     An ElementId-typed parameter used to get values from the document for a given
    //     element.
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotEqualsRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEqualsRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 433, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 433, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether integer values from the document
    //     do not equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     An integer-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotEqualsRule(ElementId parameter, int value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004b: Expected O, but got Unknown
        //IL_00dd: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_00a0: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEqualsRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 416, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 416, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether double-precision values from the
    //     document do not equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     A double-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    //   epsilon:
    //     Defines the tolerance within which two values may be considered equal.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentException:
    //     The given value for value is not finite -or- The given value for value is not
    //     a number -or- The given value for epsilon is not finite -or- The given value
    //     for epsilon is not a number
    //
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotEqualsRule(ElementId parameter, double value, double epsilon)
    {
        //IL_000b: Expected I8, but got I
        //IL_004f: Expected O, but got Unknown
        //IL_00e1: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_00a4: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEqualsRule(val, value, epsilon);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 399, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 399, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotEqualsRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEqualsRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 380, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 380, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     equal a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateNotEqualsRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEqualsRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 363, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 363, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEqualsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether ElementId values from the document
    //     are greater than a certain value.
    //
    // Parameters:
    //   parameter:
    //     An ElementId-typed parameter used to get values from the document for a given
    //     element.
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateGreaterRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 522, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 522, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether integer values from the document
    //     are greater than a certain value.
    //
    // Parameters:
    //   parameter:
    //     An integer-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateGreaterRule(ElementId parameter, int value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004b: Expected O, but got Unknown
        //IL_00dd: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_00a0: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 505, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 505, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether double-precision values from the
    //     document are greater than a certain value.
    //
    // Parameters:
    //   parameter:
    //     A double-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    //   epsilon:
    //     Defines the tolerance within which two values may be considered equal.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentException:
    //     The given value for value is not finite -or- The given value for value is not
    //     a number -or- The given value for epsilon is not finite -or- The given value
    //     for epsilon is not a number
    //
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     Values greater than value but within epsilon are considered equal, not greater.
    public unsafe static FilterRule CreateGreaterRule(ElementId parameter, double value, double epsilon)
    {
        //IL_000b: Expected I8, but got I
        //IL_004f: Expected O, but got Unknown
        //IL_00e1: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_00a4: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterRule(val, value, epsilon);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 488, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 488, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are greater
    //     than a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "greater" than another if it would appear after the other
    //     in an alphabetically-sorted list.
    public unsafe static FilterRule CreateGreaterRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 469, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 469, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are greater
    //     than a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "greater" than another if it would appear after the other
    //     in an alphabetically-sorted list.
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateGreaterRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 452, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 452, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether ElementId values from the document
    //     are greater than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     An ElementId-typed parameter used to get values from the document for a given
    //     element.
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateGreaterOrEqualRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterOrEqualRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 611, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 611, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether integer values from the document
    //     are greater than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     An integer-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateGreaterOrEqualRule(ElementId parameter, int value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004b: Expected O, but got Unknown
        //IL_00dd: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_00a0: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterOrEqualRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 594, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 594, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether double-precision values from the
    //     document are greater than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     A double-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    //   epsilon:
    //     Defines the tolerance within which two values may be considered equal.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentException:
    //     The given value for value is not finite -or- The given value for value is not
    //     a number -or- The given value for epsilon is not finite -or- The given value
    //     for epsilon is not a number
    //
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     Values less than the user-supplied value but within epsilon are considered equal;
    //     therefore, such values satisfy the condition.
    public unsafe static FilterRule CreateGreaterOrEqualRule(ElementId parameter, double value, double epsilon)
    {
        //IL_000b: Expected I8, but got I
        //IL_004f: Expected O, but got Unknown
        //IL_00e1: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_00a4: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterOrEqualRule(val, value, epsilon);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 577, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 577, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are greater
    //     than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "greater" than another if it would appear after the other
    //     in an alphabetically-sorted list.
    public unsafe static FilterRule CreateGreaterOrEqualRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterOrEqualRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 558, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 558, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are greater
    //     than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "greater" than another if it would appear after the other
    //     in an alphabetically-sorted list.
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateGreaterOrEqualRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateGreaterOrEqualRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 541, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 541, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateGreaterOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether ElementId values from the document
    //     are less than a certain value.
    //
    // Parameters:
    //   parameter:
    //     An ElementId-typed parameter used to get values from the document for a given
    //     element.
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateLessRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 700, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 700, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether integer values from the document
    //     are less than a certain value.
    //
    // Parameters:
    //   parameter:
    //     An integer-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateLessRule(ElementId parameter, int value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004b: Expected O, but got Unknown
        //IL_00dd: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_00a0: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 683, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 683, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether double-precision values from the
    //     document are less than a certain value.
    //
    // Parameters:
    //   parameter:
    //     A double-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    //   epsilon:
    //     Defines the tolerance within which two values may be considered equal.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentException:
    //     The given value for value is not finite -or- The given value for value is not
    //     a number -or- The given value for epsilon is not finite -or- The given value
    //     for epsilon is not a number
    //
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     Values less than the user-supplied value but within epsilon are considered equal,
    //     not less.
    public unsafe static FilterRule CreateLessRule(ElementId parameter, double value, double epsilon)
    {
        //IL_000b: Expected I8, but got I
        //IL_004f: Expected O, but got Unknown
        //IL_00e1: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_00a4: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessRule(val, value, epsilon);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 666, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 666, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are less
    //     than a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "less" than another if it would appear before the other
    //     in an alphabetically-sorted list.
    public unsafe static FilterRule CreateLessRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 647, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 647, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are less
    //     than a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "less" than another if it would appear before the other
    //     in an alphabetically-sorted list.
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateLessRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 630, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 630, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether ElementId values from the document
    //     are less than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     An ElementId-typed parameter used to get values from the document for a given
    //     element.
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateLessOrEqualRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessOrEqualRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 789, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 789, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether integer values from the document
    //     are less than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     An integer-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateLessOrEqualRule(ElementId parameter, int value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004b: Expected O, but got Unknown
        //IL_00dd: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_0090: Expected I, but got I8
        //IL_00a0: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessOrEqualRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 772, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 772, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether double-precision values from the
    //     document are less than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     A double-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied value against which values from the document will be compared.
    //
    //
    //   epsilon:
    //     Defines the tolerance within which two values may be considered equal.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentException:
    //     The given value for value is not finite -or- The given value for value is not
    //     a number -or- The given value for epsilon is not finite -or- The given value
    //     for epsilon is not a number
    //
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     Values greater than the user-supplied value but within epsilon are considered
    //     equal; therefore, such values satisfy the condition.
    public unsafe static FilterRule CreateLessOrEqualRule(ElementId parameter, double value, double epsilon)
    {
        //IL_000b: Expected I8, but got I
        //IL_004f: Expected O, but got Unknown
        //IL_00e1: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_0094: Expected I, but got I8
        //IL_00a4: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessOrEqualRule(val, value, epsilon);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 755, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 755, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are less
    //     than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "less" than another if it would appear before the other
    //     in an alphabetically-sorted list.
    public unsafe static FilterRule CreateLessOrEqualRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessOrEqualRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 736, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 736, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document are less
    //     than or equal to a certain value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value against which values from the document will be
    //     compared.
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    //
    // Remarks:
    //     For strings, a value is "less" than another if it would appear before the other
    //     in an alphabetically-sorted list.
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateLessOrEqualRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateLessOrEqualRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 719, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 719, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateLessOrEqualRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document contain
    //     a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateContainsRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateContainsRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 825, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateContainsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 825, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateContainsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document contain
    //     a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateContainsRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateContainsRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 808, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateContainsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 808, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateContainsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     contain a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotContainsRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotContainsRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 861, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotContainsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 861, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotContainsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     contain a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateNotContainsRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotContainsRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 844, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotContainsRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 844, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotContainsRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document begin
    //     with a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateBeginsWithRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateBeginsWithRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 897, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateBeginsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 897, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateBeginsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document begin
    //     with a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateBeginsWithRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateBeginsWithRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 880, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateBeginsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 880, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateBeginsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     begin with a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotBeginsWithRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotBeginsWithRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 933, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotBeginsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 933, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotBeginsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     begin with a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateNotBeginsWithRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotBeginsWithRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 916, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotBeginsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 916, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotBeginsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document end with
    //     a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateEndsWithRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEndsWithRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 969, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEndsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 969, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEndsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document end with
    //     a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateEndsWithRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateEndsWithRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 952, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEndsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 952, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateEndsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     end with a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    // Returns:
    //     Created filter rule object.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateNotEndsWithRule(ElementId parameter, string value)
    {
        //IL_000b: Expected I8, but got I
        //IL_004e: Expected O, but got Unknown
        //IL_00e0: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_0093: Expected I, but got I8
        //IL_00a3: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEndsWithRule(val, value);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1005, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEndsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1005, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEndsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether strings from the document do not
    //     end with a certain string value.
    //
    // Parameters:
    //   parameter:
    //     A string-typed parameter used to get values from the document for a given element.
    //
    //
    //   value:
    //     The user-supplied string value for which values from the document will be searched.
    //
    //
    //   caseSensitive:
    //     If true, the string comparison will be case-sensitive.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    [Obsolete("This method is deprecated in Revit 2023 and may be removed in a future version of Revit. Please use the constructor without the `caseSensitive` argument instead.")]
    public unsafe static FilterRule CreateNotEndsWithRule(ElementId parameter, string value, [MarshalAs(UnmanagedType.U1)] bool caseSensitive)
    {
        //IL_000b: Expected I8, but got I
        //IL_0052: Expected O, but got Unknown
        //IL_00e4: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_0097: Expected I, but got I8
        //IL_00a7: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            string text = null;
            text = value;
            bool flag = caseSensitive;
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateNotEndsWithRule(val, value, caseSensitive);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 988, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEndsWithRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 988, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateNotEndsWithRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether a parameter is associated with
    //     a certain global parameter.
    //
    // Parameters:
    //   parameter:
    //     A parameter that can be associated with an existing global parameter of a compatible
    //     type.
    //
    //   value:
    //     The global parameter used to test the association.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateIsAssociatedWithGlobalParameterRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateIsAssociatedWithGlobalParameterRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1022, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateIsAssociatedWithGlobalParameterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1022, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateIsAssociatedWithGlobalParameterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether a parameter is not associated with
    //     a certain global parameter.
    //
    // Parameters:
    //   parameter:
    //     A parameter that can be associated with an existing global parameter of a compatible
    //     type.
    //
    //   value:
    //     The global parameter used to test the association.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateIsNotAssociatedWithGlobalParameterRule(ElementId parameter, ElementId value)
    {
        //IL_000c: Expected I8, but got I
        //IL_0063: Expected O, but got Unknown
        //IL_00f7: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00a8: Expected I, but got I8
        //IL_00b9: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            ElementIdProxy val2 = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            val2 = ((!(value == null)) ? value.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateIsNotAssociatedWithGlobalParameterRule(val, val2);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype5);
            _api_publictype_tag_commontype api_publictype_tag_commontype6 = api_publictype_tag_commontype5;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val3)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val3);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1039, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateIsNotAssociatedWithGlobalParameterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1039, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateIsNotAssociatedWithGlobalParameterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether an element's parameter has a value.
    //
    //
    // Parameters:
    //   parameter:
    //     The parameter to be evaluated by the filter.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateHasValueParameterRule(ElementId parameter)
    {
        //IL_000b: Expected I8, but got I
        //IL_0047: Expected O, but got Unknown
        //IL_00d9: Expected I, but got I8
        //IL_008c: Expected I, but got I8
        //IL_008c: Expected I, but got I8
        //IL_009c: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateHasValueParameterRule(val);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1054, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateHasValueParameterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1054, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateHasValueParameterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    //
    // Summary:
    //     Creates a filter rule that determines whether an element's parameter does not
    //     have a value.
    //
    // Parameters:
    //   parameter:
    //     The parameter to be evaluated by the filter.
    //
    // Exceptions:
    //   T:Autodesk.Revit.Exceptions.ArgumentNullException:
    //     A non-optional argument was null
    public unsafe static FilterRule CreateHasNoValueParameterRule(ElementId parameter)
    {
        //IL_000b: Expected I8, but got I
        //IL_0047: Expected O, but got Unknown
        //IL_00d9: Expected I, but got I8
        //IL_008c: Expected I, but got I8
        //IL_008c: Expected I, but got I8
        //IL_009c: Expected I, but got I8
        FilterRule filterRule = null;
        long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
        try
        {
            ElementIdProxy val = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype);
            _api_publictype_tag_commontype api_publictype_tag_commontype2 = api_publictype_tag_commontype;
            val = ((!(parameter == null)) ? parameter.getProxy() : null);
            FilterRuleProxy pProxyObj = ParameterFilterRuleFactoryProxy.CreateHasNoValueParameterRule(val);
            filterRule = null;
            System.Runtime.CompilerServices.Unsafe.SkipInit(out _api_publictype_tag_commontype api_publictype_tag_commontype3);
            _api_publictype_tag_commontype api_publictype_tag_commontype4 = api_publictype_tag_commontype3;
            global::_003CModule_003E.PublicAPICommonTypeConverter_002EProxyToPublic_003Cclass_0020Autodesk_003A_003ARevit_003A_003AProxy_003A_003ADB_003A_003AFilterRuleProxy_002Cclass_0020Autodesk_003A_003ARevit_003A_003ADB_003A_003AFilterRule_003E(pProxyObj, ref filterRule);
            return filterRule;
        }
        catch (ApplicationExceptionProxy val2)
        {
            throw ExceptionConvertor.GeneratePublicAPIException(val2);
        }
        catch (Exception ex)
        {
            if (ex is Autodesk.Revit.Exceptions.ApplicationException)
            {
                throw;
            }

            throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1069, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateHasNoValueParameterRule"), ex);
        }
        catch when (((Func<bool>)delegate
        {
            // Could not convert BlockContainer to single expression
            uint exceptionCode = (uint)Marshal.GetExceptionCode();
            return (byte)global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), null, 0, null) != 0;
        }).Invoke())
        {
            uint num2 = 0u;
            global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
            try
            {
                try
                {
                    throw ExceptionCreator.CreateDefaultInternalException(new Autodesk.Revit.Exceptions.FunctionId("E:\\Ship\\2023_px64\\Source\\API\\RevitAPI\\gensrc\\APIParameterFilterElement.cpp", 1069, "Autodesk::Revit::DB::ParameterFilterRuleFactory::CreateHasNoValueParameterRule"));
                }
                catch when (((Func<bool>)delegate
                {
                    // Could not convert BlockContainer to single expression
                    num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
                    return (byte)num2 != 0;
                }).Invoke())
                {
                }

                if (num2 != 0)
                {
                    throw;
                }
            }
            finally
            {
                global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
            }
        }

        return null;
    }

    internal ParameterFilterRuleFactoryProxy getProxy()
    {
        return m_proxy;
    }

    internal ParameterFilterRuleFactoryProxy getProxyAsParameterFilterRuleFactoryProxy()
    {
        return m_proxy;
    }

    [HandleProcessCorruptedStateExceptions]
    protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
    {
        if (A_0)
        {
            ReleaseUnmanagedResources(disposing: true);
            return;
        }

        try
        {
            ReleaseUnmanagedResources(disposing: false);
        }
        finally
        {
            base.Finalize();
        }
    }

    public virtual sealed void Dispose()
    {
        Dispose(A_0: true);
        GC.SuppressFinalize(this);
    }

    ~ParameterFilterRuleFactory()
    {
        Dispose(A_0: false);
    }
}
#if false // Decompilation log
'19' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
------------------
Resolve: 'AdpSDKCSharpWrapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'AdpSDKCSharpWrapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'AdskRcManaged, Version=1.0.7977.26735, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'AdskRcManaged, Version=1.0.7977.26735, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Could not find by name: 'PresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
------------------
Resolve: 'PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Could not find by name: 'PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\System.Core.dll'
------------------
Resolve: 'WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Could not find by name: 'WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\System.dll'
------------------
Resolve: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'RevitAPIFoundation, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'RevitAPIFoundation, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'RevitDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'RevitDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'RevitDBCoreAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'RevitDBCoreAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'PersistenceDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'PersistenceDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'UtilityAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'UtilityAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'APIDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'APIDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'GraphicsAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'GraphicsAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'ElementGroupDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'ElementGroupDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'EssentialsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'EssentialsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'GeomUtilAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'GeomUtilAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'FamilyDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'FamilyDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'DetailDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'DetailDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'CurtainGridFamilyDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'CurtainGridFamilyDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'ArrayElemsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'ArrayElemsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'EnergyAnalysisDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'EnergyAnalysisDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'BuildingSystemsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'BuildingSystemsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'HostObjDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'HostObjDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'RoomAreaPlanDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'RoomAreaPlanDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'SiteDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'SiteDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'SculptingDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'SculptingDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'StructuralDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'StructuralDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'InterfaceAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'InterfaceAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'InterfaceUtilAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'InterfaceUtilAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'RebarDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'RebarDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'StructuralAnalysisDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'StructuralAnalysisDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'MassingDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'MassingDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'AssemblyDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'AssemblyDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'DPartDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'DPartDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'NumberingDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'NumberingDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'PointCloudAccessAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'PointCloudAccessAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'StairRampDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'StairRampDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'AnalysisAppsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'AnalysisAppsDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'MaterialDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'MaterialDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'IntfATFAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'IntfATFAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'TextEngineAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'TextEngineAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'InfrastructureDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'InfrastructureDBAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'ATFRevitCoreAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'ATFRevitCoreAPI, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'DBManagedServices, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
Could not find by name: 'DBManagedServices, Version=23.0.0.0, Culture=neutral, PublicKeyToken=null'
------------------
Resolve: 'System.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
#endif

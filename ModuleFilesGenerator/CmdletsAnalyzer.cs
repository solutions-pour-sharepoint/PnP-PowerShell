﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Runtime.Serialization;
using SharePointPnP.PowerShell.CmdletHelpAttributes;
using SharePointPnP.PowerShell.ModuleFilesGenerator.Model;
using CmdletInfo = SharePointPnP.PowerShell.ModuleFilesGenerator.Model.CmdletInfo;

namespace SharePointPnP.PowerShell.ModuleFilesGenerator
{
    internal class CmdletsAnalyzer
    {
        private readonly Assembly _assembly;

        internal CmdletsAnalyzer(Assembly assembly)
        {
            _assembly = assembly;
        }

        internal List<CmdletInfo> Analyze()
        {

            return GetCmdlets();

        }
        private List<CmdletInfo> GetCmdlets()
        {
            List<CmdletInfo> cmdlets = new List<CmdletInfo>();
            var types = _assembly.GetTypes().Where(t => t.BaseType != null && (t.BaseType.Name.StartsWith("SPO") || t.BaseType.Name.StartsWith("PnP") || t.BaseType.Name == "PSCmdlet")).OrderBy(t => t.Name).ToArray();

            foreach (var type in types)
            {
                var cmdletInfo = new Model.CmdletInfo();
                cmdletInfo.CmdletType = type;

                var attributes = type.GetCustomAttributes();

                foreach (var attribute in attributes)
                {
                    var cmdletAttribute = attribute as CmdletAttribute;
                    if (cmdletAttribute != null)
                    {
                        var a = cmdletAttribute;
                        cmdletInfo.Verb = a.VerbName;
                        cmdletInfo.Noun = a.NounName;

                    }
                    var aliasAttribute = attribute as CmdletAliasAttribute;
                    if (aliasAttribute != null)
                    {
                        cmdletInfo.Aliases.Add(aliasAttribute.Alias);
                    }

                    var helpAttribute = attribute as CmdletHelpAttribute;
                    if (helpAttribute != null)
                    {
                        var a = helpAttribute;
                        cmdletInfo.Description = a.Description;
                        cmdletInfo.Copyright = a.Copyright;
                        cmdletInfo.Version = a.Version;
                        cmdletInfo.DetailedDescription = a.DetailedDescription;
                        cmdletInfo.Category = ToEnumString(a.Category);
                        cmdletInfo.OutputType = a.OutputType;
                        cmdletInfo.OutputTypeLink = a.OutputTypeLink;
                        cmdletInfo.OutputTypeDescription = a.OutputTypeDescription;
                    }
                    var exampleAttribute = attribute as CmdletExampleAttribute;
                    if (exampleAttribute != null)
                    {
                        cmdletInfo.Examples.Add(exampleAttribute);
                    }
                    var linkAttribute = attribute as CmdletRelatedLinkAttribute;
                    if (linkAttribute != null)
                    {
                        cmdletInfo.RelatedLinks.Add(linkAttribute);
                    }
                    var additionalParameter = attribute as CmdletAdditionalParameter;
                    if (additionalParameter != null)
                    {
                        cmdletInfo.AdditionalParameters.Add(additionalParameter);
                    }
                }
                if (!string.IsNullOrEmpty(cmdletInfo.Verb) && !string.IsNullOrEmpty(cmdletInfo.Noun))
                {
                    cmdletInfo.Syntaxes = GetCmdletSyntaxes(cmdletInfo);
                    cmdletInfo.Parameters = GetCmdletParameters(cmdletInfo);
                    cmdlets.Add(cmdletInfo);
                }
            }

            return cmdlets;
        }

        private List<CmdletSyntax> GetCmdletSyntaxes(Model.CmdletInfo cmdletInfo)
        {
            List<CmdletSyntax> syntaxes = new List<CmdletSyntax>();
            var fields = GetFields(cmdletInfo.CmdletType);
            foreach (var field in fields)
            {
                MemberInfo fieldInfo = field;
                var obsolete = fieldInfo.GetCustomAttributes<ObsoleteAttribute>().Any();

                if (!obsolete)
                {
                    var parameterAttributes = fieldInfo.GetCustomAttributes<ParameterAttribute>(true).Where(a => a.ParameterSetName != ParameterAttribute.AllParameterSets);
                    foreach (var parameterAttribute in parameterAttributes)
                    {
                        var cmdletSyntax = syntaxes.FirstOrDefault(c => c.ParameterSetName == parameterAttribute.ParameterSetName);
                        if (cmdletSyntax == null)
                        {
                            cmdletSyntax = new CmdletSyntax();
                            cmdletSyntax.ParameterSetName = parameterAttribute.ParameterSetName;
                            syntaxes.Add(cmdletSyntax);
                        }
                        var typeString = field.FieldType.Name;
                        var fieldAttribute = field.FieldType.GetCustomAttributes<CmdletPipelineAttribute>(false).FirstOrDefault();
                        if (fieldAttribute != null)
                        {
                            if (fieldAttribute.Type != null)
                            {
                                typeString = string.Format(fieldAttribute.Description, fieldAttribute.Type.Name);
                            }
                            else
                            {
                                typeString = fieldAttribute.Description;
                            }
                        }

                        cmdletSyntax.Parameters.Add(new CmdletParameterInfo()
                        {
                            Name = field.Name,
                            Description = parameterAttribute.HelpMessage,
                            Position = parameterAttribute.Position,
                            Required = parameterAttribute.Mandatory,
                            Type = typeString
                        });
                    }
                }
            }

            foreach (var additionalParameter in cmdletInfo.AdditionalParameters.Where(a => a.ParameterSetName != ParameterAttribute.AllParameterSets))
            {
                var cmdletSyntax = syntaxes.FirstOrDefault(c => c.ParameterSetName == additionalParameter.ParameterSetName);
                if (cmdletSyntax == null)
                {
                    cmdletSyntax = new CmdletSyntax();
                    cmdletSyntax.ParameterSetName = additionalParameter.ParameterSetName;
                    syntaxes.Add(cmdletSyntax);
                }
                var typeString = additionalParameter.ParameterType.Name;
                var fieldAttribute = additionalParameter.ParameterType.GetCustomAttributes<CmdletPipelineAttribute>(false).FirstOrDefault();
                if (fieldAttribute != null)
                {
                    if (fieldAttribute.Type != null)
                    {
                        typeString = string.Format(fieldAttribute.Description, fieldAttribute.Type.Name);
                    }
                    else
                    {
                        typeString = fieldAttribute.Description;
                    }
                }
                cmdletSyntax.Parameters.Add(new CmdletParameterInfo()
                {
                    Name = additionalParameter.ParameterName,
                    Description = additionalParameter.HelpMessage,
                    Position = additionalParameter.Position,
                    Required = additionalParameter.Mandatory,
                    Type = typeString
                });
            }

            // AllParameterSets
            foreach (var field in fields)
            {
                var obsolete = field.GetCustomAttributes<ObsoleteAttribute>().Any();

                if (!obsolete)
                {
                    var parameterAttributes = field.GetCustomAttributes<ParameterAttribute>(true).Where(a => a.ParameterSetName == ParameterAttribute.AllParameterSets);
                    foreach (var parameterAttribute in parameterAttributes)
                    {
                        if (!syntaxes.Any())
                        {
                            syntaxes.Add(new CmdletSyntax { ParameterSetName = ParameterAttribute.AllParameterSets });
                        }

                        foreach (var syntax in syntaxes)
                        {
                            var typeString = field.FieldType.Name;
                            var fieldAttribute = field.FieldType.GetCustomAttributes<CmdletPipelineAttribute>(false).FirstOrDefault();
                            if (fieldAttribute != null)
                            {
                                if (fieldAttribute.Type != null)
                                {
                                    typeString = string.Format(fieldAttribute.Description, fieldAttribute.Type.Name);
                                }
                                else
                                {
                                    typeString = fieldAttribute.Description;
                                }
                            }
                            syntax.Parameters.Add(new CmdletParameterInfo()
                            {
                                Name = field.Name,
                                Description = parameterAttribute.HelpMessage,
                                Position = parameterAttribute.Position,
                                Required = parameterAttribute.Mandatory,
                                Type = typeString
                            });
                        }
                    }
                }
            }
            return syntaxes;
        }

        private List<CmdletParameterInfo> GetCmdletParameters(Model.CmdletInfo cmdletInfo)
        {
            List<CmdletParameterInfo> parameters = new List<CmdletParameterInfo>();
            var fields = GetFields(cmdletInfo.CmdletType);
            foreach (var field in fields)
            {
                MemberInfo fieldInfo = field;
                var obsolete = fieldInfo.GetCustomAttributes<ObsoleteAttribute>().Any();

                if (!obsolete)
                {
                    var parameterAttributes = fieldInfo.GetCustomAttributes<ParameterAttribute>(true).ToArray();
                    foreach (var parameterAttribute in parameterAttributes)
                    {
                        var description = parameterAttribute.HelpMessage;
                        if (string.IsNullOrEmpty(description))
                        {
                            // Maybe a generic one? Find the one with only a helpmessage set
                            var helpParameterAttribute = parameterAttributes.FirstOrDefault(p => !string.IsNullOrEmpty(p.HelpMessage));
                            if (helpParameterAttribute != null)
                            {
                                description = helpParameterAttribute.HelpMessage;
                            }
                        }
                        var typeString = field.FieldType.Name;
                        var fieldAttribute = field.FieldType.GetCustomAttributes<CmdletPipelineAttribute>(false).FirstOrDefault();
                        if (fieldAttribute != null)
                        {
                            if (fieldAttribute.Type != null)
                            {
                                typeString = string.Format(fieldAttribute.Description, fieldAttribute.Type.Name);
                            }
                            else
                            {
                                typeString = fieldAttribute.Description;
                            }
                        }
                        parameters.Add(new CmdletParameterInfo()
                        {
                            Description = description,
                            Type = typeString,
                            Name = field.Name,
                            Required = parameterAttribute.Mandatory,
                            Position = parameterAttribute.Position,
                        });
                    }
                }
            }

            foreach (var additionalParameter in cmdletInfo.AdditionalParameters)
            {
                var typeString = additionalParameter.ParameterType.Name;
                var fieldAttribute = additionalParameter.ParameterType.GetCustomAttributes<CmdletPipelineAttribute>(false).FirstOrDefault();
                if (fieldAttribute != null)
                {
                    if (fieldAttribute.Type != null)
                    {
                        typeString = string.Format(fieldAttribute.Description, fieldAttribute.Type.Name);
                    }
                    else
                    {
                        typeString = fieldAttribute.Description;
                    }
                }
                parameters.Add(new CmdletParameterInfo()
                {
                    Description = additionalParameter.HelpMessage,
                    Type = typeString,
                    Name = additionalParameter.ParameterName,
                    Required = additionalParameter.Mandatory,
                    Position = additionalParameter.Position,
                });
            }

            return parameters;
        }



        #region Helpers
        private static List<FieldInfo> GetFields(Type t)
        {
            var fieldInfoList = new List<FieldInfo>();
            foreach (var fieldInfo in t.GetFields())
            {
                fieldInfoList.Add(fieldInfo);
            }
            if (t.BaseType != null && t.BaseType.BaseType != null)
            {
                fieldInfoList.AddRange(GetFields(t.BaseType.BaseType));
            }
            return fieldInfoList;
        }

        private static string ToEnumString<T>(T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            try
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                return enumMemberAttribute.Value;
            }
            catch
            {
                return name;
            }
        }
        #endregion
    }
}

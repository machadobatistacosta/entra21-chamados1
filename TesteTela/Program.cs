﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace ConsoleApplication
{
    public class Program
    {

        //Checar nomenclatura métodos
        //Checar nomenclatura propriedades
        //Se checkbox de Entity for marcado, checar pra ver se existe um campo id inteiro
        //Se checkbox de Entity for marcado, checar pra ver se a classe tem um construtor sem parâmetro


        static Action<string> Write = Console.WriteLine;

        public static void Main(string[] args)
        {
            Write("Let's compile!");

            string codeToCompile = @"
            using System;

            namespace RoslynCompileSample
            {
                public class Writer
                {
                    public void Write(string message)
                    {
                        Console.WriteLine($""you said '{message}!'"");
                    }
                    public string Nome {get;set;}
                    public int Dado {get;set;}

                }
            }";

            Write("Parsing the code into the SyntaxTree");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(codeToCompile);

            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            Write("Adding the following references");
            foreach (var r in refPaths)
                Write(r);

            Write("Compiling ...");
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    Write("Compilation failed!");
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("\t{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    Write("Compilation successful! Now instantiating and executing the code ...");
                    ms.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    var type = assembly.GetType("RoslynCompileSample.Writer");
                    
                    MethodInfo[] metodos = type.GetMethods();
                    ConstructorInfo[] construtores = type.GetConstructors();

                    foreach(MethodInfo metodos1 in metodos)
                    {
                        if(metodos1.Name)
                    }
                    foreach (var item in construtores)
                    {
                        if (item.GetParameters().Length == 0)
                        {

                        }
                    }

                    PropertyInfo[] propriedades = type.GetProperties();
                    foreach (PropertyInfo p in propriedades)
                    {
                        if (p.Name == "ID" && p.PropertyType == typeof(int))
                        {

                        }
                    }


                    var instance = assembly.CreateInstance("RoslynCompileSample.Writer");
                    var meth = type.GetMember("Write").First() as MethodInfo;
                    meth.Invoke(instance, new[] { "joel" });
                }
            }


        }
    }
}
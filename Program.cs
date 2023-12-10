using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

// Задача 1: Рефлексия для класса Console
class ReflectionTask1
{
    public static void PrintConsoleMethods()
    {
        Type consoleType = typeof(Console);
        MethodInfo[] methods = consoleType.GetMethods();

        Console.WriteLine("Методы класса Console:");
        foreach (var method in methods)
        {
            Console.WriteLine(method.Name);
        }
    }
}

// Задача 2: Рефлексия для свойств класса
class MyClass
{
    public int MyProperty1 { get; set; }
    public string MyProperty2 { get; set; }
}

class ReflectionTask2
{
    public static void PrintProperties()
    {
        MyClass myObject = new MyClass { MyProperty1 = 42, MyProperty2 = "Hello" };

        Type myType = myObject.GetType();
        PropertyInfo[] properties = myType.GetProperties();

        Console.WriteLine("Свойства класса MyClass и их значения:");
        foreach (var property in properties)
        {
            Console.WriteLine($"{property.Name}: {property.GetValue(myObject)}");
        }
    }
}

// Задача 3: Вызов метода Substring через рефлексию
class ReflectionTask3
{
    public static void CallSubstring()
    {
        string myString = "Hello, Reflection!";
        Type stringType = typeof(string);

        MethodInfo substringMethod = stringType.GetMethod("Substring", new Type[] { typeof(int), typeof(int) });
        object[] parameters = { 7, 10 };
        string result = (string)substringMethod.Invoke(myString, parameters);

        Console.WriteLine($"Результат вызова Substring: {result}");
    }
}

// Задача 4: Получение списка конструкторов List<T>
class ReflectionTask4
{
    public static void PrintListConstructors()
    {
        Type listType = typeof(List<>);
        Type genericArgument = typeof(int);
        Type concreteListType = listType.MakeGenericType(genericArgument);

        ConstructorInfo[] constructors = concreteListType.GetConstructors();

        Console.WriteLine($"Конструкторы List<{genericArgument}>:");
        foreach (var constructor in constructors)
        {
            Console.WriteLine(constructor);
        }
    }
}

// Дополнительная задача 1: Работа с классами из Student.dll
interface IStudent
{
    void PrintInfo();
    int Age { get; set; }
}

class ReflectionAdditionalTask1
{
    public static void WorkWithStudentDll()
    {
        Assembly studentAssembly = Assembly.LoadFrom("Student.dll");

        Type studentType = studentAssembly.GetType("Student.Student");
        object studentObject = Activator.CreateInstance(studentType);

        MethodInfo method = studentType.GetMethod("PrintInfo");
        method.Invoke(studentObject, null);

        PropertyInfo property = studentType.GetProperty("Age");
        Console.WriteLine($"Текущий возраст: {property.GetValue(studentObject)}");

        property.SetValue(studentObject, 25);
        Console.WriteLine($"Новый возраст: {property.GetValue(studentObject)}");
    }
}

// Дополнительная задача 2: Получение цепочки предков и интерфейсов
class ReflectionAdditionalTask2
{
    public static void PrintAncestryAndInterfaces()
    {
        Assembly studentAssembly = Assembly.LoadFrom("Student.dll");

        Type studentType = studentAssembly.GetType("Student.Student");

        Console.WriteLine($"Цепочка предков для класса {studentType.Name}:");

        Type currentType = studentType;
        while (currentType != null)
        {
            Console.WriteLine(currentType);
            currentType = currentType.BaseType;
        }

        Console.WriteLine("\nРеализуемые интерфейсы:");

        foreach (var interfaceType in studentType.GetInterfaces())
        {
            Console.WriteLine(interfaceType);
        }
    }
}

// Дополнительная задача 3: Динамическое создание и использование Dictionary<string,int>
class ReflectionAdditionalTask3
{
    public static void DynamicDictionary()
    {
        Type dictionaryType = typeof(Dictionary<,>);
        Type[] typeArgs = { typeof(string), typeof(int) };
        Type concreteDictType = dictionaryType.MakeGenericType(typeArgs);

        object dictObject = Activator.CreateInstance(concreteDictType);

        MethodInfo addMethod = concreteDictType.GetMethod("Add");

        addMethod.Invoke(dictObject, new object[] { "One", 1 });
        addMethod.Invoke(dictObject, new object[] { "Two", 2 });
        addMethod.Invoke(dictObject, new object[] { "Three", 3 });

        MethodInfo getItemMethod = concreteDictType.GetMethod("get_Item");
        int value = (int)getItemMethod.Invoke(dictObject, new object[] { "Two" });

        Console.WriteLine($"Значение для ключа 'Two': {value}");
    }
}

// Дополнительная задача 4: Программа автоподключения плагинов
interface IPlugin
{
    void Execute();
}

class ReflectionAdditionalTask4
{
    public static void AutoLoadPlugins()
    {
        string pluginDirectory = "Plugins";

        if (Directory.Exists(pluginDirectory))
        {
            string[] pluginFiles = Directory.GetFiles(pluginDirectory, "*.dll");

            foreach (var pluginFile in pluginFiles)
            {
                Assembly pluginAssembly = Assembly.LoadFrom(pluginFile);

                foreach (var type in pluginAssembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type))
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        plugin.Execute();
                    }
                }
            }
        }
        else
        {
            Console.WriteLine($"Директория {pluginDirectory} не существует.");
        }
    }
}

class Program
{
    static void Main()
    {
        // Задача 1: Рефлексия для класса Console
        ReflectionTask1.PrintConsoleMethods();

        // Задача 2: Рефлексия для свойств класса
        ReflectionTask2.PrintProperties();

        // Задача 3: Вызов метода Substring через рефлексию
        ReflectionTask3.CallSubstring();

        // Задача 4: Получение списка конструкторов List<T>
        ReflectionTask4.PrintListConstructors();

        // Дополнительная задача 1: Работа с классами из Student.dll
        ReflectionAdditionalTask1.WorkWithStudentDll();

        // Дополнительная задача 2: Получение цепочки предков и интерфейсов
        ReflectionAdditionalTask2.PrintAncestryAndInterfaces();

        // Дополнительная задача 3: Динамическое создание и использование Dictionary<string,int>
        ReflectionAdditionalTask3.DynamicDictionary();

        // Дополнительная задача 4: Программа автоподключения плагинов
        ReflectionAdditionalTask4.AutoLoadPlugins();

        // Ожидание ввода перед завершением программы
        Console.ReadLine();
    }
}

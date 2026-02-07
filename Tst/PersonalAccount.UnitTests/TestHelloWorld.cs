using NUnit.Framework;
using PersonalAccount.Domain;

namespace PersonalAccount.UnitTests;

public class TestHelloWorld
{
    [Test]
    public void Check_HelloWorld()
    {
        // Подготовка
        var class1 = new Class1();

        // Действие
        var result = class1.GetHelloWorld();

        // Проверка
        Assert.That( string.IsNullOrWhiteSpace(result) == false );
    }
    
    [Ignore("-")]
    [Test]
    public void FailCheck_HelloWorld()
    {
        Assert.Fail("Test failed");
    }
}

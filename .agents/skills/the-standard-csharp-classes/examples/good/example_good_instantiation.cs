// ---
// skill: the-standard-csharp-classes
// type: example
// source-section: "2. Classes and Interfaces — 4.2 Instantiations"
// demonstrates: "tsc-csharp-classes-007, tsc-csharp-classes-008 — named aliases and property order"
// ---

// ✅ GOOD: Named aliases for literals; property initializer order matches class declaration order.

public class Student
{
    public Guid Id { get; set; }   // declared first
    public string Name { get; set; } // declared second
}

// ✅ Named alias when using literal values
var student = new Student(name: "Josh", score: 150);

// ✅ Object initializer properties match declaration order (Id first, then Name)
var student = new Student
{
    Id = Guid.NewGuid(),
    Name = "Elbek"
};

// ✅ Constructor with named parameters in declaration order
public class Student
{
    private readonly Guid id;
    private readonly string name;

    public Student(Guid id, string name)
    {
        this.id = id;
        this.name = name;
    }
}

var student = new Student(id: Guid.NewGuid(), name: "Elbek");

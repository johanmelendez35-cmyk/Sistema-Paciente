using System.ComponentModel.DataAnnotations;

namespace PatientSystem;

public class Patient
{
    public int Id { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = "";

    [StringLength(50)]
    public string LastName { get; set; } = "";

    [StringLength(11)]
    public string NationalId { get; set; } = "";

    public int Age { get; set; }

    [StringLength(15)]
    public string Phone { get; set; } = "";

    [StringLength(100)]
    public string Address { get; set; } = "";

    [StringLength(200)]
    public string Diagnosis { get; set; } = "";

    public bool HasInsurance { get; set; }

    public Patient()
    {
    }

    public Patient(string firstName, string lastName, string nationalId, int age,
                   string phone, string address, string diagnosis, bool hasInsurance)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalId = nationalId;
        Age = age;
        Phone = phone;
        Address = address;
        Diagnosis = diagnosis;
        HasInsurance = hasInsurance;
    }

    public string FullName => FirstName + " " + LastName;
}

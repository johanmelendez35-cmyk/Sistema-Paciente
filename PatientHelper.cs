using Microsoft.EntityFrameworkCore;

namespace PatientSystem;

public static class PatientHelper
{
    public static string ReadText(string message, int maxLength)
    {
        string result = "";
        bool isValid = false;

        while (!isValid)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";
            input = input.Trim();

            if (input == "")
            {
                Console.WriteLine("  Error: este campo es obligatorio.");
            }
            else if (input.Length > maxLength)
            {
                Console.WriteLine("  Error: máximo " + maxLength + " caracteres.");
            }
            else
            {
                result = input;
                isValid = true;
            }
        }

        return result;
    }

    public static int ReadInt(string message, int min, int max)
    {
        int result = 0;
        bool isValid = false;

        while (!isValid)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            try
            {
                result = int.Parse(input);

                if (result < min || result > max)
                {
                    Console.WriteLine("  Error: el número debe estar entre " + min + " y " + max + ".");
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("  Error: digite un número entero.");
            }
        }

        return result;
    }

    public static string ReadNationalId()
    {
        string nationalId = "";
        bool isValid = false;

        while (!isValid)
        {
            nationalId = ReadText("Cédula (11 dígitos, sin guiones): ", 11);
            isValid = true;

            if (nationalId.Length != 11)
            {
                isValid = false;
            }

            foreach (char character in nationalId)
            {
                if (character < '0' || character > '9')
                {
                    isValid = false;
                }
            }

            if (!isValid)
            {
                Console.WriteLine("  Error: la cédula debe tener exactamente 11 dígitos.");
            }
        }

        return nationalId;
    }

    public static bool ReadYesNo(string message)
    {
        int option = ReadInt(message + " (1. Sí, 2. No): ", 1, 2);
        return option == 1;
    }

    public static void AddPatient()
    {
        Console.WriteLine("=== AGREGAR NUEVO PACIENTE ===");

        string nationalId = ReadNationalId();

        try
        {
            ApplicationDbContext context = new ApplicationDbContext();

            if (context.Patients.Any(p => p.NationalId == nationalId))
            {
                Console.WriteLine("Ya existe un paciente registrado con esa cédula.");
                return;
            }

            string firstName = ReadText("Nombre: ", 50);
            string lastName = ReadText("Apellido: ", 50);
            int age = ReadInt("Edad: ", 0, 120);
            string phone = ReadText("Teléfono: ", 15);
            string address = ReadText("Dirección: ", 100);
            string diagnosis = ReadText("Diagnóstico: ", 200);
            bool hasInsurance = ReadYesNo("¿Tiene seguro médico?");

            Patient patient = new Patient(firstName, lastName, nationalId, age,
                                          phone, address, diagnosis, hasInsurance);

            context.Patients.Add(patient);
            context.SaveChanges();

            Console.WriteLine("Paciente registrado satisfactoriamente.");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Error: no se pudo guardar el paciente en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inesperado: " + ex.Message);
        }
    }

    public static void ListPatients()
    {
        Console.WriteLine("=== LISTADO DE PACIENTES ===");

        try
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<Patient> patients = context.Patients.ToList();

            if (patients.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }

            ShowTableHeader();

            foreach (Patient patient in patients)
            {
                ShowPatientRow(patient);
            }

            Console.WriteLine("\nTotal: " + patients.Count + " paciente(s).");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al consultar la base de datos: " + ex.Message);
        }
    }

    public static void SearchPatient()
    {
        Console.WriteLine("=== BUSCAR PACIENTE ===");
        string text = ReadText("Digite nombre, apellido o cédula: ", 50);

        try
        {
            ApplicationDbContext context = new ApplicationDbContext();

            List<Patient> found = context.Patients
                .Where(p => p.FirstName.Contains(text)
                         || p.LastName.Contains(text)
                         || p.NationalId.Contains(text))
                .ToList();

            if (found.Count == 0)
            {
                Console.WriteLine("No se encontraron pacientes con ese criterio.");
                return;
            }

            ShowTableHeader();

            foreach (Patient patient in found)
            {
                ShowPatientRow(patient);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al consultar la base de datos: " + ex.Message);
        }
    }

    public static void EditPatient()
    {
        Console.WriteLine("=== EDITAR PACIENTE ===");
        int id = ReadInt("ID del paciente que desea modificar: ", 1, int.MaxValue);

        try
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var patient = context.Patients.FirstOrDefault(p => p.Id == id);

            if (patient != null)
            {
                Console.WriteLine("Editando a: " + patient.FullName);

                patient.FirstName = ReadText("Nuevo nombre: ", 50);
                patient.LastName = ReadText("Nuevo apellido: ", 50);
                patient.Age = ReadInt("Nueva edad: ", 0, 120);
                patient.Phone = ReadText("Nuevo teléfono: ", 15);
                patient.Address = ReadText("Nueva dirección: ", 100);
                patient.Diagnosis = ReadText("Nuevo diagnóstico: ", 200);
                patient.HasInsurance = ReadYesNo("¿Tiene seguro médico?");

                context.Patients.Update(patient);
                context.SaveChanges();

                Console.WriteLine("Paciente actualizado satisfactoriamente.");
            }
            else
            {
                Console.WriteLine("No se encontró un paciente con ese ID.");
            }
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Error: no se pudo actualizar el paciente en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inesperado: " + ex.Message);
        }
    }

    public static void DeletePatient()
    {
        Console.WriteLine("=== ELIMINAR PACIENTE ===");
        int id = ReadInt("ID del paciente que desea eliminar: ", 1, int.MaxValue);

        try
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var patient = context.Patients.FirstOrDefault(p => p.Id == id);

            if (patient != null)
            {
                Console.WriteLine("Va a eliminar a: " + patient.FullName + " (Cédula " + patient.NationalId + ")");
                bool confirmed = ReadYesNo("¿Está seguro?");

                if (confirmed)
                {
                    context.Patients.Remove(patient);
                    context.SaveChanges();
                    Console.WriteLine("Paciente eliminado satisfactoriamente.");
                }
                else
                {
                    Console.WriteLine("Operación cancelada.");
                }
            }
            else
            {
                Console.WriteLine("No se encontró un paciente con ese ID.");
            }
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Error: no se pudo eliminar el paciente de la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error inesperado: " + ex.Message);
        }
    }

    public static void ShowStatistics()
    {
        Console.WriteLine("=== ESTADÍSTICAS ===");

        try
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<Patient> patients = context.Patients.ToList();

            if (patients.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }

            int withInsurance = patients.Count(p => p.HasInsurance);
            int withoutInsurance = patients.Count - withInsurance;

            int ageSum = 0;
            foreach (Patient patient in patients)
            {
                ageSum = ageSum + patient.Age;
            }
            double averageAge = (double)ageSum / patients.Count;

            Console.WriteLine("Total de pacientes : " + patients.Count);
            Console.WriteLine("Con seguro médico  : " + withInsurance);
            Console.WriteLine("Sin seguro médico  : " + withoutInsurance);
            Console.WriteLine("Edad promedio      : " + averageAge.ToString("F1") + " años");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al consultar la base de datos: " + ex.Message);
        }
    }

    private static void ShowTableHeader()
    {
        Console.WriteLine($"{"ID",-4} {"Nombre",-22} {"Cédula",-12} {"Edad",-5} {"Teléfono",-13} {"Seguro",-6}");
        Console.WriteLine(new string('-', 66));
    }

    private static void ShowPatientRow(Patient patient)
    {
        string insurance = patient.HasInsurance ? "Sí" : "No";

        Console.WriteLine($"{patient.Id,-4} {patient.FullName,-22} {patient.NationalId,-12} {patient.Age,-5} {patient.Phone,-13} {insurance,-6}");
    }
}

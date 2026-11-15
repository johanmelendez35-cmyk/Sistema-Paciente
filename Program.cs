using PatientSystem;

bool keepRunning = true;

DisplayHeader();

while (keepRunning)
{
    DisplayMenu();

    int selectedOption = PatientHelper.ReadInt("Seleccione una opción: ", 1, 7);
    Console.WriteLine();

    switch (selectedOption)
    {
        case 1:
            PatientHelper.AddPatient();
            break;
        case 2:
            PatientHelper.ListPatients();
            break;
        case 3:
            PatientHelper.SearchPatient();
            break;
        case 4:
            PatientHelper.EditPatient();
            break;
        case 5:
            PatientHelper.DeletePatient();
            break;
        case 6:
            PatientHelper.ShowStatistics();
            break;
        case 7:
            keepRunning = false;
            Console.WriteLine("Cerrando la aplicación.");
            break;
    }

    if (keepRunning)
    {
        Console.WriteLine("\nPresione ENTER para continua.");
        Console.ReadLine();
        Console.Clear();
    }
}

void DisplayHeader()
{
    Console.WriteLine("=========================================");
    Console.WriteLine("   Clinica Calderón - Sistema de Pacientes");
    Console.WriteLine("=========================================");
}

void DisplayMenu()
{
    Console.WriteLine("\n--------------- MENÚ ---------------");
    Console.WriteLine("1. Agregar paciente");
    Console.WriteLine("2. Listar pacientes");
    Console.WriteLine("3. Buscar paciente");
    Console.WriteLine("4. Editar paciente");
    Console.WriteLine("5. Eliminar paciente");
    Console.WriteLine("6. Estadísticas");
    Console.WriteLine("7. Salir");
    Console.WriteLine("------------------------------------");
}

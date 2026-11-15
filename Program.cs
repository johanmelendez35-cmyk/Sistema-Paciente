using SistemaPacientes;


bool continuar = true;

DisplayHeader();

while (continuar)
{
    DisplayMenu();

    int opcionSeleccionada = PacienteHelper.LeerEntero("Seleccione una opción: ", 1, 7);
    Console.WriteLine();

    switch (opcionSeleccionada)
    {
        case 1:
            PacienteHelper.AgregarPaciente();
            break;
        case 2:
            PacienteHelper.ListarPacientes();
            break;
        case 3:
            PacienteHelper.BuscarPaciente();
            break;
        case 4:
            PacienteHelper.EditarPaciente();
            break;
        case 5:
            PacienteHelper.EliminarPaciente();
            break;
        case 6:
            PacienteHelper.MostrarEstadisticas();
            break;
        case 7:
            continuar = false;
            Console.WriteLine("Cerrando la aplicación. ¡Hasta luego!");
            break;
    }

    if (continuar)
    {
        Console.WriteLine("\nPresione ENTER para continuar...");
        Console.ReadLine();
        Console.Clear();
    }
}

void DisplayHeader()
{
    Console.WriteLine("=========================================");
    Console.WriteLine("   SISTEMA DE REGISTRO DE PACIENTES");
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

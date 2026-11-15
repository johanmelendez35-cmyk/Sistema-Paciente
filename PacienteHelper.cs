using Microsoft.EntityFrameworkCore;

namespace SistemaPacientes;

public static class PacienteHelper
{
   
    public static string LeerTexto(string mensaje, int longitudMaxima)
    {
        string resultado = "";
        bool valido = false;

        while (!valido)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (entrada == null || entrada.Trim() == "")
            {
                Console.WriteLine("  Error: este campo es obligatorio.");
            }
            else if (entrada.Trim().Length > longitudMaxima)
            {
                Console.WriteLine($"  Error: máximo {longitudMaxima} caracteres.");
            }
            else
            {
                resultado = entrada.Trim();
                valido = true;
            }
        }

        return resultado;
    }

    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        int resultado = 0;
        bool valido = false;

        while (!valido)
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out resultado))
            {
                Console.WriteLine("  Error: digite un número entero.");
            }
            else if (resultado < minimo || resultado > maximo)
            {
                Console.WriteLine($"  Error: el número debe estar entre {minimo} y {maximo}.");
            }
            else
            {
                valido = true;
            }
        }

        return resultado;
    }

    public static string LeerCedula()
    {
        string cedula = "";
        bool valida = false;

        while (!valida)
        {
            cedula = LeerTexto("Cédula (11 dígitos, sin guiones): ", 11);

            if (cedula.Length == 11 && cedula.All(char.IsDigit))
            {
                valida = true;
            }
            else
            {
                Console.WriteLine("  Error: la cédula debe tener exactamente 11 dígitos.");
            }
        }

        return cedula;
    }

    public static bool LeerSiNo(string mensaje)
    {
        int opcion = LeerEntero(mensaje + " (1. Sí, 2. No): ", 1, 2);
        return opcion == 1;
    }

    public static void AgregarPaciente()
    {
        Console.WriteLine("=== AGREGAR NUEVO PACIENTE ===");

        string cedula = LeerCedula();

        try
        {
            using var contexto = new ApplicationDbContext();

            if (contexto.Pacientes.Any(p => p.Cedula == cedula))
            {
                Console.WriteLine("Ya existe un paciente registrado con esa cédula.");
                return;
            }

            string nombre = LeerTexto("Nombre: ", 50);
            string apellido = LeerTexto("Apellido: ", 50);
            int edad = LeerEntero("Edad: ", 0, 120);
            string telefono = LeerTexto("Teléfono: ", 15);
            string direccion = LeerTexto("Dirección: ", 100);
            string diagnostico = LeerTexto("Diagnóstico: ", 200);
            bool tieneSeguro = LeerSiNo("¿Tiene seguro médico?");

            Paciente paciente = new Paciente(nombre, apellido, cedula, edad,
                                             telefono, direccion, diagnostico, tieneSeguro);

            contexto.Pacientes.Add(paciente);
            contexto.SaveChanges(); // sin esto no llega a la base de datos

            Console.WriteLine("Paciente registrado satisfactoriamente.");
        }
        catch (DbUpdateException)
        {
            Console.WriteLine("Error: no se pudo guardar el paciente en la base de datos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
        }
    }

    public static void ListarPacientes()
    {
        Console.WriteLine("=== LISTADO DE PACIENTES ===");

        try
        {
            using var contexto = new ApplicationDbContext();
            List<Paciente> pacientes = contexto.Pacientes.ToList();

            if (pacientes.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }

            MostrarEncabezadoTabla();

            foreach (Paciente paciente in pacientes)
            {
                MostrarFilaPaciente(paciente);
            }

            Console.WriteLine($"\nTotal: {pacientes.Count} paciente(s).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar la base de datos: {ex.Message}");
        }
    }

    public static void BuscarPaciente()
    {
        Console.WriteLine("=== BUSCAR PACIENTE ===");
        string texto = LeerTexto("Digite nombre, apellido o cédula: ", 50);

        try
        {
            using var contexto = new ApplicationDbContext();

            List<Paciente> encontrados = contexto.Pacientes
                .Where(p => p.Nombre.Contains(texto)
                         || p.Apellido.Contains(texto)
                         || p.Cedula.Contains(texto))
                .ToList();

            if (encontrados.Count == 0)
            {
                Console.WriteLine("No se encontraron pacientes con ese criterio.");
                return;
            }

            MostrarEncabezadoTabla();

            foreach (Paciente paciente in encontrados)
            {
                MostrarFilaPaciente(paciente);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar la base de datos: {ex.Message}");
        }
    }

    public static void EditarPaciente()
    {
        Console.WriteLine("=== EDITAR PACIENTE ===");
        int id = LeerEntero("ID del paciente que desea modificar: ", 1, int.MaxValue);

        try
        {
            using var contexto = new ApplicationDbContext();

            Paciente? paciente = contexto.Pacientes.FirstOrDefault(p => p.Id == id);

            if (paciente != null)
            {
                Console.WriteLine($"Editando a: {paciente.NombreCompleto}");

                paciente.Nombre = LeerTexto("Nuevo nombre: ", 50);
                paciente.Apellido = LeerTexto("Nuevo apellido: ", 50);
                paciente.Edad = LeerEntero("Nueva edad: ", 0, 120);
                paciente.Telefono = LeerTexto("Nuevo teléfono: ", 15);
                paciente.Direccion = LeerTexto("Nueva dirección: ", 100);
                paciente.Diagnostico = LeerTexto("Nuevo diagnóstico: ", 200);
                paciente.TieneSeguro = LeerSiNo("¿Tiene seguro médico?");

                contexto.Pacientes.Update(paciente);
                contexto.SaveChanges();

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
            Console.WriteLine($"Error inesperado: {ex.Message}");
        }
    }

    public static void EliminarPaciente()
    {
        Console.WriteLine("=== ELIMINAR PACIENTE ===");
        int id = LeerEntero("ID del paciente que desea eliminar: ", 1, int.MaxValue);

        try
        {
            using var contexto = new ApplicationDbContext();
            Paciente? paciente = contexto.Pacientes.FirstOrDefault(p => p.Id == id);

            if (paciente != null)
            {
                Console.WriteLine($"Va a eliminar a: {paciente.NombreCompleto} (Cédula {paciente.Cedula})");
                bool confirmado = LeerSiNo("¿Está seguro?");

                if (confirmado)
                {
                    contexto.Pacientes.Remove(paciente);
                    contexto.SaveChanges();
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
            Console.WriteLine($"Error inesperado: {ex.Message}");
        }
    }

    public static void MostrarEstadisticas()
    {
        Console.WriteLine("=== ESTADÍSTICAS ===");

        try
        {
            using var contexto = new ApplicationDbContext();

            int total = contexto.Pacientes.Count();

            if (total == 0)
            {
                Console.WriteLine("No hay pacientes registrados.");
                return;
            }

            int conSeguro = contexto.Pacientes.Count(p => p.TieneSeguro);
            int sinSeguro = total - conSeguro;
            double edadPromedio = contexto.Pacientes.Average(p => p.Edad);

            Console.WriteLine($"Total de pacientes : {total}");
            Console.WriteLine($"Con seguro médico  : {conSeguro}");
            Console.WriteLine($"Sin seguro médico  : {sinSeguro}");
            Console.WriteLine($"Edad promedio      : {edadPromedio:F1} años");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consultar la base de datos: {ex.Message}");
        }
    }

    private static void MostrarEncabezadoTabla()
    {
        Console.WriteLine($"{"ID",-4} {"Nombre",-22} {"Cédula",-12} {"Edad",-5} {"Teléfono",-13} {"Seguro",-6}");
        Console.WriteLine(new string('-', 66));
    }

    private static void MostrarFilaPaciente(Paciente paciente)
    {
        string seguro = paciente.TieneSeguro ? "Sí" : "No";

        Console.WriteLine($"{paciente.Id,-4} {paciente.NombreCompleto,-22} {paciente.Cedula,-12} {paciente.Edad,-5} {paciente.Telefono,-13} {seguro,-6}");
    }
}

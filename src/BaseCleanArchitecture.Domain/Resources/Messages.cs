namespace BaseCleanArchitecture.Domain.Resources
{
    public static class Messages
    {
        // Mensajes de Usuario
        public static string UsuarioNoEncontrado => "El usuario no fue encontrado";
        public static string UsuarioNoValido => "Usuario no válido";
        public static string NoSePudoObtenerIdUsuario => "No se pudo obtener el ID del usuario del token";
        public static string NoSePudoDeterminarEmpresa => "No se pudo determinar la empresa del usuario";
        public static string ContraseñaInvalida => "La contraseña debe tener al menos 8 caracteres";
        public static string EmailYContraseñaRequeridos => "Email y contraseña son requeridos";
        public static string CredencialesInvalidas => "Credenciales inválidas";
        public static string NoSePuedeEliminar => "No se puede eliminar el usuario porque tiene relaciones activas";

        // Mensajes de Validación
        public static string TelefonoInvalido => "El teléfono debe tener entre 10 y 13 dígitos";
        public static string NombreInvalido => "El nombre debe tener al menos 3 caracteres";
        public static string RFCInvalido => "El RFC debe tener 13 caracteres";
        public static string CorreoInvalido => "El correo electrónico no es válido";

        // Mensajes de Empresa
        public static string EmpresaNoEncontrada => "La empresa no fue encontrada";
        public static string EmpresaNoValida => "Empresa no válida";

        // Mensajes de Sucursal
        public static string SucursalNoEncontrada => "La sucursal no fue encontrada";
        public static string SucursalNoValida => "Sucursal no válida";
        public static string SucursalYaExiste => "Ya existe una sucursal con ese nombre en la empresa";

        // Mensajes de Departamento
        public static string DepartamentoNoEncontrado => "El departamento no fue encontrado";
        public static string DepartamentoNoValido => "Departamento no válido";
        public static string DepartamentoYaExiste => "Ya existe un departamento con ese nombre en la empresa";

        // Mensajes de Tipo de Unidad
        public static string TipoUnidadNoEncontrado => "El tipo de unidad no fue encontrado";
        public static string TipoUnidadNoValido => "Tipo de unidad no válido";
        public static string TipoUnidadClaveExiste => "Ya existe un tipo de unidad con esa clave en la empresa";
        public static string TipoUnidadNombreExiste => "Ya existe un tipo de unidad con ese nombre en la empresa";

        // Mensajes de Almacén
        public static string AlmacenNoEncontrado => "El almacén no fue encontrado";
        public static string AlmacenNoValido => "Almacén no válido";
        public static string AlmacenYaExiste => "Ya existe un almacén con ese nombre en la sucursal";

        // Mensajes de Ubicación
        public static string UbicacionNoEncontrada => "La ubicación no fue encontrada";
        public static string UbicacionNoValida => "Ubicación no válida";
        public static string UbicacionYaExiste => "Ya existe una ubicación con ese código en el almacén";

        // Mensajes de Artículo
        public static string ArticuloNoEncontrado => "El artículo no fue encontrado";
        public static string ArticuloNoValido => "Artículo no válido";
        public static string ArticuloYaExiste => "Ya existe un artículo con ese nombre en la empresa";

        // Mensajes de Cliente
        public static string ClienteNoEncontrado => "El cliente no fue encontrado";
        public static string ClienteNoValido => "Cliente no válido";
        public static string ClienteYaExiste => "Ya existe un cliente con ese RFC en la empresa";

        // Mensajes de Error General
        public static string ErrorInternoServidor => "Error interno del servidor";
        public static string RecursoNoEncontrado => "El recurso solicitado no fue encontrado";
        public static string NoAutorizado => "No autorizado para realizar esta acción";
        public static string ValidacionFallida => "La validación ha fallado";
        public static string OperacionNoPermitida => "Operación no permitida";
        public static string DatosInvalidos => "Los datos proporcionados no son válidos";
    }
} 
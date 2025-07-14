namespace BaseCleanArchitecture.Application.DTOs
{
    /// <summary>
    /// Respuesta estándar para todos los endpoints de la API
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Mensaje { get; set; } = "Operación exitosa";
        public T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(T? data)
        {
            Data = data;
        }

        public static ApiResponse<T> SuccessResponse(T? data, string mensaje = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Mensaje = mensaje,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string mensaje, T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Mensaje = mensaje,
                Data = data
            };
        }
    }

    /// <summary>
    /// Respuesta para operaciones que no devuelven datos específicos
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public ErrorResponse? Error { get; set; }

        public static ApiResponse SuccessResponse(string mensaje = "Operación exitosa")
        {
            return new ApiResponse
            {
                Success = true,
                Mensaje = mensaje,
                Error = null
            };
        }

        public static ApiResponse ErrorResponse(int codigo, string mensaje, List<string> detalles)
        {
            return new ApiResponse
            {
                Success = false,
                Mensaje = string.Empty,
                Error = new ErrorResponse
                {
                    Codigo = codigo,
                    Mensaje = mensaje,
                    Detalles = detalles
                }
            };
        }

        public static ApiResponse ErrorResponse(int codigo, string mensaje, string detalle)
        {
            return new ApiResponse
            {
                Success = false,
                Mensaje = string.Empty,
                Error = new ErrorResponse
                {
                    Codigo = codigo,
                    Mensaje = mensaje,
                    Detalles = new List<string> { detalle }
                }
            };
        }
    }

    /// <summary>
    /// Información detallada del error
    /// </summary>
    public class ErrorResponse
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<string> Detalles { get; set; } = new List<string>();
    }

    public class ApiError
    {
        public bool Success { get; set; } = false;
        public string Mensaje { get; set; } = string.Empty;
        public int Codigo { get; set; }
        public List<string> Detalles { get; set; } = new();

        public ApiError(string mensaje, int codigo = 400)
        {
            Mensaje = mensaje;
            Codigo = codigo;
        }

        public static ApiError Create(string mensaje, int codigo = 400)
        {
            return new ApiError(mensaje, codigo);
        }
    }
} 
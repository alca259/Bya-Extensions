using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// Representa un número. En la clase se desglosan las distintas opciones que se puedan
    /// encontrar
    /// </summary>
    internal class NumeroNif
    {
        #region Propiedades
        /// <summary>
        /// Tipos de Códigos.
        /// </summary>
        /// <remarks>Aunque actualmente no se utilice el término CIF, se usa en la enumeración por comodidad</remarks>
        private enum EnumDocumentTypes { NIF, NIE, CIF }

        // Número tal cual lo introduce el usuario
        private readonly string _number;
        private EnumDocumentTypes DocumentType { get; set; }

        /// <summary>
        /// Parte de Nif: En caso de ser un Nif intracomunitario, permite obtener el cógido del país
        /// </summary>
        private string CodigoIntracomunitario { get; set; }
        private bool EsIntraComunitario { get; set; }

        /// <summary>
        /// Parte de Nif: Letra inicial del Nif, en caso de tenerla
        /// </summary>
        private string LetraInicial { get; set; }

        /// <summary>
        /// Parte de Nif: Bloque numérico del NIF. En el caso de un NIF de persona física,
        /// corresponderá al DNI
        /// </summary>
        private int Number { get; set; }

        /// <summary>
        /// Parte de Nif: Dígito de control. Puede ser número o letra
        /// </summary>
        private string DigitoControl { get; set; }

        /// <summary>
        /// Valor que representa si el Nif introducido es correcto
        /// </summary>
        public bool EsCorrecto { get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor. Al instanciar la clase se realizan todos los cálculos
        /// </summary>
        /// <param name="number">Cadena de 9 u 11 caracteres que contiene el DNI/NIF
        /// tal cual lo ha introducido el usuario para su verificación</param>
        private NumeroNif(string number)
        {
            // Comprobación básica de la cadena introducida por el usuario
            if (string.IsNullOrEmpty(number))
            {
                throw new ArgumentException("El documento introducido está vacío y no puede validarse");
            }

            // Se eliminan los carácteres sobrantes
            number = DeleteBadCharacters(number);

            // En maýusculas
            number = number.ToUpper();

            if (number.Substring(0, 1) == "X") number = 0 + number.Substring(1, 8);
            if (number.Substring(0, 1) == "Y") number = 1 + number.Substring(1, 8);
            if (number.Substring(0, 1) == "Z") number = 2 + number.Substring(1, 8);

            // Comprobación básica de la cadena introducida por el usuario
            if (number.Length != 9 && number.Length != 11)
            {
                throw new ArgumentException("El documento introducido no tiene un número de caracteres válidos");
            }

            _number = number;
            ExplodeNumber();

            switch (DocumentType)
            {
                case EnumDocumentTypes.NIF:
                case EnumDocumentTypes.NIE:
                    EsCorrecto = CheckDocumentNif();
                    break;
                case EnumDocumentTypes.CIF:
                    EsCorrecto = CheckDocumentCif();
                    break;
            }

            if (!EsCorrecto)
            {
                throw new ArgumentException($"El documento de tipo {DocumentType} no es correcto.");
            }
        }
        #endregion

        #region Limpieza y preparación del número
        /// <summary>
        /// Realiza un desglose del número introducido por el usuario en las propiedades
        /// de la clase
        /// </summary>
        private void ExplodeNumber()
        {
            int n;

            if (_number.Length == 11)
            {
                // Nif Intracomunitario
                EsIntraComunitario = true;
                CodigoIntracomunitario = _number.Substring(0, 2);
                LetraInicial = _number.Substring(2, 1);
                int.TryParse(_number.Substring(3, 7), out n);
                DigitoControl = _number.Substring(10, 1);
                DocumentType = GetDocumentType(LetraInicial[0]);
            }
            else
            {
                // Nif español
                DocumentType = GetDocumentType(_number[0]);
                EsIntraComunitario = false;
                if (DocumentType == EnumDocumentTypes.NIF)
                {
                    LetraInicial = string.Empty;
                    if (!int.TryParse(_number.Substring(0, 8), out n))
                    {
                        throw new ArgumentException($"Los dígitos de la posición 0 a la posición 8 del documento de tipo {DocumentType}, no son números");
                    }
                }
                else
                {
                    LetraInicial = _number.Substring(0, 1);
                    if (!int.TryParse(_number.Substring(1, 7), out n))
                    {
                        throw new ArgumentException($"Los dígitos de la posición 1 a la posición 7 del documento de tipo {DocumentType}, no son números");
                    }
                }
                DigitoControl = _number.Substring(8, 1);
            }

            Number = n;
        }

        /// <summary>
        /// En base al primer carácter del código, se obtiene el tipo de documento que se intenta
        /// comprobar
        /// </summary>
        /// <param name="letra">Primer carácter del número pasado</param>
        /// <returns>Tipo de documento</returns>
        private static EnumDocumentTypes GetDocumentType(char letra)
        {
            var regexNumeros = new Regex("[0-9]");
            if (regexNumeros.IsMatch(letra.ToString()))
                return EnumDocumentTypes.NIF;

            var regexLetrasNIE = new Regex("[XYZ]");
            if (regexLetrasNIE.IsMatch(letra.ToString()))
                return EnumDocumentTypes.NIE;

            var regexLetrasCIF = new Regex("[ABCDEFGHJPQRSUVNW]");
            if (regexLetrasCIF.IsMatch(letra.ToString()))
                return EnumDocumentTypes.CIF;

            throw new ApplicationException("El código no es reconocible");
        }

        /// <summary>
        /// Eliminación de todos los carácteres no numéricos o de texto de la cadena
        /// </summary>
        /// <param name="pNumero">Número tal cual lo escribe el usuario</param>
        /// <returns>Cadena de 9 u 11 carácteres sin signos</returns>
        private static string DeleteBadCharacters(string pNumero)
        {
            // Todos los carácteres que no sean números o letras
            const string caracteres = @"[^\w]";
            var regex = new Regex(caracteres);
            return regex.Replace(pNumero, "");
        }
        #endregion

        #region Cálculos
        private bool CheckDocumentNif()
        {
            return DigitoControl == GetLetraNif();
        }

        /// <summary>
        /// Cálculos para la comprobación del Cif (Entidad jurídica)
        /// </summary>
        private bool CheckDocumentCif()
        {
            string[] letrasCodigo = { "J", "A", "B", "C", "D", "E", "F", "G", "H", "I" };

            var n = Number.ToString("0000000");
            var sumaPares = 0;
            var sumaImpares = 0;
            var sumaTotal = 0;
            var i = 0;
            var retVal = false;

            // Recorrido por todos los dígitos del número
            for (i = 0; i < n.Length; i++)
            {
                int.TryParse(n[i].ToString(), out var aux);

                if ((i + 1) % 2 == 0)
                {
                    // Si es una posición par, se suman los dígitos
                    sumaPares += aux;
                }
                else
                {
                    // Si es una posición impar, se multiplican los dígitos por 2 
                    aux = aux * 2;

                    // se suman los dígitos de la suma
                    sumaImpares += SumDigits(aux);
                }
            }
            // Se suman los resultados de los números pares e impares
            sumaTotal += sumaPares + sumaImpares;

            // Se obtiene el dígito de las unidades
            var unidades = sumaTotal % 10;

            // Si las unidades son distintas de 0, se restan de 10
            if (unidades != 0)
                unidades = 10 - unidades;

            switch (LetraInicial)
            {
                // Sólo números
                case "A":
                case "B":
                case "E":
                case "H":
                    retVal = DigitoControl == unidades.ToString();
                    break;

                // Sólo letras
                case "K":
                case "P":
                case "Q":
                case "S":
                    retVal = DigitoControl == letrasCodigo[unidades];
                    break;

                default:
                    retVal = DigitoControl == unidades.ToString()
                            || DigitoControl == letrasCodigo[unidades];
                    break;
            }

            return retVal;

        }

        /// <summary>
        /// Obtiene la suma de todos los dígitos
        /// </summary>
        /// <returns>de 23, devuelve la suma de 2 + 3</returns>
        private static int SumDigits(int digitos)
        {
            var sNumero = digitos.ToString();
            var suma = 0;

            for (var i = 0; i < sNumero.Length; i++)
            {
                int.TryParse(sNumero[i].ToString(), out var aux);
                suma += aux;
            }
            return suma;
        }

        /// <summary>
        /// Obtiene la letra correspondiente al Dni
        /// </summary>
        private string GetLetraNif()
        {
            var indice = Number % 23;
            return "TRWAGMYFPDXBNJZSQVHLCKET"[indice].ToString();
        }
        #endregion

        /// <summary>
        /// Obtiene una cadena con el número de identificación completo
        /// </summary>
        public override string ToString()
        {
            var formato = "{0:0000000}";

            switch (DocumentType)
            {
                case EnumDocumentTypes.CIF when LetraInicial == "":
                    formato = "{0:00000000}";
                    break;
                case EnumDocumentTypes.NIF:
                    formato = "{0:00000000}";
                    break;
            }

            var nif = EsIntraComunitario
                ? CodigoIntracomunitario
                : string.Empty + LetraInicial + string.Format(formato, Number) + DigitoControl;

            return nif;
        }

        /// <summary>
        /// Comprobación de un número de identificación fiscal español
        /// </summary>
        /// <param name="numero">Numero a analizar</param>
        /// <returns>Instancia de <see cref="NumeroNif"/> con los datos del número.
        /// Destacable la propiedad <seealso cref="EsCorrecto"/>, que contiene la verificación
        /// </returns>
        public static NumeroNif CheckDocumentNif(string numero)
        {
            return new NumeroNif(numero);
        }
    }
}

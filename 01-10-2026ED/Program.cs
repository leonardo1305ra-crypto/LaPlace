// See https://aka.ms/new-console-template for more information
using static System.Net.Mime.MediaTypeNames;
Console.OutputEncoding = System.Text.Encoding.UTF8; 

Console.WriteLine("TRANSFORMACIONES DE LAPLACE\n");
Console.WriteLine("Formula a utilizar: ∫[0,∞] e^(-st)f(t)dt\n");

string L = "e^(-st)"; //Esto es lo que siempre se multiplica en Laplace
string iL = "-(1/s)e^-st"; //La integral de L
string funcion; //Lo que escribe el usuario

//Pide la funcion y no deja que la dejen vacia
do
{
    Console.WriteLine("ingresa la funcion");
    funcion = Console.ReadLine();
}
while (string.IsNullOrWhiteSpace(funcion));

string[] partes = funcion.Split(' '); //Separa la funcion por espacios, por si hay sumas

string sMultiplicacion = ""; //Aqui se va armando la multiplicacion con e^(-st)


if (partes.Length == 1)
{
    string parte = partes[0];
    sMultiplicacion = $"{L}({parte})"; //Multiplicamos la funcion por e^(-st)
    Console.WriteLine($"Funcion ingresada: f(t) = {parte}");
    Console.WriteLine("Sustitucion...");
    Console.WriteLine($"∫[0,∞]{sMultiplicacion}dt\n");

    int constante = 0;

    //CASO 1: por partes
    if (parte.StartsWith("t"))
    {
        int n = 0; //El exponente de la t
        if (parte == "t")
        {
            n = 1; //Si solo puso t, el exponente es 1
        }
        else if (parte.StartsWith("t^"))
        {
            int.TryParse(parte.Substring(2), out n); //Saca el numero que esta despues del ^
        }

        if (n >= 1)
        {
            int factorial = 1; //multiplica n * (n-1) * ... * 1
            string cadena = ""; //junta las fracciones

            Console.WriteLine("Esta integral no se puede hacer directo, se hace por partes");
            Console.WriteLine("Formula: ∫u dv = uv - ∫v du\n");

            //Repetimos por partes hasta que ya no quede t
            for (int k = n; k >= 1; k--)
            {
                string tk = "t"; //La t que tenemos ahorita
                if (k > 1) tk = "t^" + k;

                string tk1 = ""; //La t que queda despues de derivar
                if (k == 2) tk1 = "t";
                if (k > 2) tk1 = "t^" + (k - 1);

                string tb = "b"; //La t cuando la cambiamos por b
                if (k > 1) tb = "b^" + k;

                string t0 = "0"; //La t cuando la cambiamos por 0
                if (k > 1) t0 = "0^" + k;

                Console.WriteLine($"----- Paso {n - k + 1} -----");
                Console.WriteLine($"∫[0,∞]{tk}{L}dt");
                Console.WriteLine($"Elegimos u = {tk}  y  dv = {L}dt");
                Console.WriteLine("Derivamos u e integramos dv");
                if (k == 1)
                {
                    Console.WriteLine($"du = dt     v = {iL}");
                }
                else
                {
                    Console.WriteLine($"du = {k}{tk1}dt     v = {iL}");
                }
                Console.WriteLine("Aplicamos ∫u dv = uv - ∫v du");
                Console.WriteLine($"= [-({tk}/s){L}][0,∞] + ({k}/s)∫[0,∞]{tk1}{L}dt");
                Console.WriteLine("Evaluamos el primer pedazo con limite");
                Console.WriteLine($"lim b->∞ [-({tb}/s)e^(-s*b)] - [-({t0}/s)e^(-s*0)]");
                Console.WriteLine("Como s > 0, el de b se hace 0 y el de 0 vale 0");
                Console.WriteLine("[0] - [0] = 0");
                Console.WriteLine($"Nos queda: ({k}/s)∫[0,∞]{tk1}{L}dt\n");

                factorial = factorial * k;
                cadena = cadena + $"({k}/s)";
            }

            //Al final queda la integral de L 
            Console.WriteLine("Al final solo queda la integral de e^(-st)");
            Console.WriteLine($"∫[0,∞]{L}dt = 1/s");
            cadena = cadena + "(1/s)";
            Console.WriteLine("Juntamos todo lo que fuimos sacando");
            Console.WriteLine($"{cadena} = {factorial}/s^{n + 1}\n");

            Console.WriteLine("RESULTADO FINAL");
            Console.WriteLine($"L{{{parte}}} = {factorial}/s^{n + 1}, s > 0");
        }
        else
        {
            Console.WriteLine("Escribe t o t^n (ej. t^2)");
        }
    }
    //CASO 2: si empieza con e^ es una exponencial (e^2t, e^(-3t), e^t)
    else if (parte.StartsWith("e^"))
    {
        //Le quitamos todo menos el numero que acompaña a la t
        string num = parte.Replace("e^", "").Replace("(", "").Replace(")", "").Replace("t", "");
        int a = 0;
        bool valido = true;

        if (num == "")
        {
            a = 1; //e^t es lo mismo que e^1t
        }
        else if (num == "-")
        {
            a = -1; //e^-t es lo mismo que e^-1t
        }
        else
        {
            valido = int.TryParse(num, out a); //no es valido si nos numero
        }

        if (valido)
        {
            //Al juntar los exponentes queda s - a
            string den = "s";
            if (a > 0) den = $"s-{a}";
            if (a < 0) den = $"s+{-a}";

            Console.WriteLine($"Es una exponencial e^(at) con a = {a}");
            Console.WriteLine("Multiplicacion...");
            Console.WriteLine($"∫[0,∞]e^({a}t){L}dt");
            Console.WriteLine("Juntamos los exponentes (se suman)");
            Console.WriteLine($"∫[0,∞]e^({a}t-st)dt");
            Console.WriteLine("Sacamos t de factor comun");
            Console.WriteLine($"∫[0,∞]e^(-({den})t)dt");
            Console.WriteLine("Integramos");
            Console.WriteLine($"[-(1/({den}))e^(-({den})t)][0,∞]");
            //Como llega hasta infinito, lo pasamos a un limite con b
            Console.WriteLine("Transcribimos a integral impropia");
            Console.WriteLine($"lim b->∞ [-(1/({den}))e^(-({den})t)][0,b]");
            //Evaluamos limites
            Console.WriteLine("Evaluamos limites");
            Console.WriteLine($"lim b->∞ [-(1/({den}))e^(-({den})*b)] - [-(1/({den}))e^(-({den})*0)]");
            Console.WriteLine($"Como s > {a}, e^(-({den})*b) -> 0 cuando b -> ∞ y e^(-({den})*0) = 1");
            Console.WriteLine($"[0] - [-(1/({den}))(1)]");
            Console.WriteLine("Menos por menos es mas, simplificamos");
            Console.WriteLine($"= 1/({den})\n");

            Console.WriteLine("RESULTADO FINAL");
            Console.WriteLine($"L{{e^({a}t)}} = 1/({den}), s > {a}");
        }
        else
        {
            Console.WriteLine("Escribe la exponencial asi: e^2t o e^(2t)");
        }
    }
    //CASO 3: constantes
    //evaluamos si la parte es una constante
    else if (int.TryParse(parte, out constante) || (parte.Length == 1 && char.IsLetter(parte[0])))
    {
        //Si es 1 no hay que sacar nada porque 1 por L es solo L
        if (parte == "1")
        {
            Console.WriteLine($"∫[0,∞]{L}");
            //integramos
            Console.WriteLine("Integramos");
            Console.WriteLine($"∫[0,∞]{L} = {iL}");


        }
        else //Cualquier otra constante
        {
            Console.WriteLine($"∫[0,∞]{parte}{L}");
            Console.WriteLine("Sacamos la constante de la integral");
            Console.WriteLine($"{parte}∫[0,∞]{L}");
            Console.WriteLine("Integramos");
            iL = iL.Replace("1", parte); //Cambiamos el 1 por la constante
            Console.WriteLine($"{parte}∫[0,∞]{L} = {iL}");

        }
        //Como llega hasta infinito, lo pasamos a un limite con b
        Console.WriteLine("Transcribimos a integral impropia");
        Console.WriteLine($"lim b->∞ [{iL}][0,b]");
        //Evaluamos limites
        Console.WriteLine("Evaluamos limites");
        string iLb = iL.Replace("-st", "-s*b"); //Ponemos b en lugar de t
        string iL0 = iL.Replace("-st", "-s*0"); //Ponemos 0 en lugar de t
        Console.WriteLine($"lim b->∞ [{iLb}] - [{iL0}]"); //Arriba menos abajo

        //Si s es mayor que 0, e^(-s*b) se hace 0 y e^(-s*0) es 1
        Console.WriteLine("Como s > 0, e^(-s*b) -> 0 cuando b -> ∞ y e^(-s*0) = 1");
        Console.WriteLine($"[0] - [-({parte}/s)(1)]");

        //Resultado: 0 menos negativo queda positivo. Si es 0 el resultado es 0
        string resultado = parte == "0" ? "0" : $"{parte}/s";
        Console.WriteLine("RESULTADO FINAL");
        Console.WriteLine($"L{{{parte}}} = {resultado}, s > 0");

    }
    else //Si no es de ninguno de los tipos de arriba todavia no se puede
    {
        Console.WriteLine("Por ahora solo se soportan constantes, t^n y e^(at).");
    }
}
else
{ 

    //Recorre cada parte
    for (int i = 0; i < partes.Length; i++)
    {
        string parte = partes[i];
        if (parte != "+")
        {
            sMultiplicacion += $"{L}({parte})"; //Multiplica cada parte por e^(-st)
        }
        else
        {
            sMultiplicacion += $" {parte} "; //Deja el + donde estaba
        }
    }
    Console.WriteLine($"∫[0,∞]{sMultiplicacion} dt"); //Muestra la integral
}
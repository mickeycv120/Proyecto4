static void Main(string[] args)
{
    int b = 7;
    int a = (3 + 5) * 8 - (10 - 2*b) / b; // 61
    a=b+10;
    int n = 5;

    if(n == 5) {
        Console.WriteLine("5 es igual a " + n + " " + a + " " + b );
      } else {
        Console.WriteLine("5 es diferente a " + n);
      }

    if(a % 2 != 0) {
    Console.WriteLine("Es impar " + a);
    if(b == 2) {
      Console.WriteLine("b es igual a 2");
    } else if( b > 3) {
      Console.WriteLine("b es mayor a 3 y vale " + b);
    }
    else {
      Console.WriteLine("b no es igual a 2 y vale " + b);
    }
  } else {
    Console.WriteLine("Es par");
  }
}


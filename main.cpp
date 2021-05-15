#include <AngouriMath.h>
#include <iostream>


int main()
{
    AngouriMath::Entity expr("x + 2 sin(x) + 2y");
    std::cout << expr << '\n';
    AngouriMath::Entity m("[[1, 0, 70], [0, 1, 0], [0, 0, 1]] ^ -1");
    std::cout << m.Simplify();
    return 0;
}
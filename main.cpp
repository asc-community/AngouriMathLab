#include <AngouriMath.h>
#include <iostream>


int main()
{
    AngouriMath::Entity expr("x + 2 sin(x) + 2y");
    std::cout << expr;
    return 0;
}
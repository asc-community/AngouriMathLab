cmake_minimum_required (VERSION 3.8)

project ("IntegrationProject")

include(FetchContent)
FetchContent_Declare(
    AngouriMath
    GIT_REPOSITORY https://github.com/asc-community/AngouriMathLab
    GIT_TAG        origin/cpp-release
)

FetchContent_MakeAvailable(AngouriMath)

add_executable(
    main.cpp
    ${PROJECT_NAME}
)


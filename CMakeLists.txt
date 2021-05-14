cmake_minimum_required (VERSION 3.8)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

project ("IntegrationProject")

include(FetchContent)
FetchContent_Declare(
    AngouriMath
    GIT_REPOSITORY https://github.com/asc-community/AngouriMathLab
    GIT_TAG        origin/cpp-release
)

FetchContent_MakeAvailable(AngouriMath)

add_executable(
	${PROJECT_NAME}
    main.cpp
)

add_custom_command(TARGET ${PROJECT_NAME} PRE_BUILD COMMAND ${CMAKE_COMMAND} -E copy ${ANGOURIMATH_LIB} $<TARGET_FILE_DIR:${PROJECT_NAME}>)
target_link_libraries(${PROJECT_NAME} AngouriMath)

cmake_minimum_required (VERSION 3.8)

project("AngouriMath")

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

set(IS_WINDOWS CMAKE_SYSTEM_NAME STREQUAL Windows)
set(IS_LINUX CMAKE_SYSTEM_NAME STREQUAL Linux)
set(IS_OSX CMAKE_SYSTEM_NAME STREQUAL Darwin)

set(LINK_TO_RELEASE https://github.com/asc-community/AngouriMathLab/releases/download/0.0.0-poc)

### Find the right archive to download
if (${IS_WINDOWS})
    set(URL ${LINK_TO_RELEASE}/AngouriMath.CPP-windows-latest-x64.zip)
elseif(${IS_LINUX})
    set(URL ${LINK_TO_RELEASE}/AngouriMath.CPP-ubuntu-latest-x64.zip)
elseif(${IS_OSX})
    set(URL ${LINK_TO_RELEASE}/AngouriMath.CPP-macos-latest-x64.zip)
else()
    message(FATAL_ERROR "OS is not recognized")
endif()
###


set(PATH_TO_DOWNLOADED_LIB ${CMAKE_CURRENT_BINARY_DIR}/AngouriMath.CPP.Importing.zip)
set(PATH_TO_ANGOURIMATH ${CMAKE_CURRENT_BINARY_DIR}/AngouriMathLibrary)


### Download the binary if doesn't exist
if(NOT EXISTS ${PATH_TO_DOWNLOADED_LIB})
  file(DOWNLOAD ${URL} ${PATH_TO_DOWNLOADED_LIB}
       TIMEOUT 60  # seconds
       )
endif()
###


### Extract
if (NOT EXISTS ${PATH_TO_ANGOURIMATH})
    file(ARCHIVE_EXTRACT INPUT ${PATH_TO_DOWNLOADED_LIB} DESTINATION ${PATH_TO_ANGOURIMATH})
endif()
###

### Adding to the current project
add_subdirectory(${PATH_TO_ANGOURIMATH} ${CMAKE_CURRENT_BINARY_DIR}/AngouriMath.CPP.Importing)
###

add_library(AngouriMath INTERFACE)

### Extension and copy the dynamic library depending on the OS
set(BINARY_FILENAME ${CMAKE_SHARED_LIBRARY_PREFIX}AngouriMath.CPP.Exporting${CMAKE_SHARED_LIBRARY_SUFFIX})
set(OLD_LIB_PATH ${PATH_TO_ANGOURIMATH}/out-x64/${BINARY_FILENAME})
set(NEW_LIB_DIR $<TARGET_FILE_DIR:${PROJECT_NAME}>)
set(NEW_LIB_PATH ${NEW_LIB_DIR}/${BINARY_FILENAME})

# add_custom_command(
#     TARGET ${PROJECT_NAME}
#     PRE_BUILD COMMAND ${CMAKE_COMMAND} -E copy 
#         ${OLD_LIB_PATH}
#         ${NEW_LIB_DIR}
# )
###


### If the platform is OS X, we need to rename dylib file
# if (${IS_OSX})
#     add_custom_command(TARGET ${PROJECT_NAME}
#         PRE_BUILD COMMAND
#         install_name_tool -id "@rpath/${BINARY_FILENAME}" ${OLD_LIB_PATH})
# endif()
###


target_link_libraries(${PROJECT_NAME} INTERFACE AngouriMath.CPP.Exporting)
target_include_directories(${PROJECT_NAME} INTERFACE ${PATH_TO_ANGOURIMATH})
target_link_directories(${PROJECT_NAME} INTERFACE ${PATH_TO_ANGOURIMATH}/out-x64)

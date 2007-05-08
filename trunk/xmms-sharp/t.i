 %module XmmsClientInterface
 %{
 /* Includes the header in the wrapper code */
 #include "xmmsc/xmmsc_idnumbers.h"
 #include "xmmsclient.h"
 %}
 
 /* Parse the header file to generate wrappers */
 %include "xmmsc/xmmsc_idnumbers.h"
 %include "xmmsclient.h"

   M S B A M L   `   `   `  ����C  ?GreyInks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:GreyInksGreyInks  5   	       GreyInks.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:GreyInks     5   	     � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	   $%��./images/#a허상고딕B 보통)�5
   	   
������� � Title$ 
MainWindow��5   	   $	��800��6   $	��650��6)    � 	Resources 5         GreyInks.GreyConverters   EnumToStringConverter�� 5   
   %   &	          5      6#   5      ��5      �� 5   
    �� ImageSource$ images/blackboard.jpg��6   6;   .��5      �� $��0 0 0 0q�6   $
��Auto��6"   $
��Auto��6/   .+�5   
   �� $��	0 20 0 10q�6   $0��*시스템보안파트 체크리스트 V0.1��6#   $��40(�6@   
�������6c   �� 5   
   $��0 0 0 30q�6   $��20(�6&   $-��' @ 안전한 PC 환경 검토 서비스��64   5      �� 5   
   -   5        $��DiagnosticCheckList��5      $��10 10 10 10q�62   ��5      �� 5      .R�5      �� $
��Auto��6&   $��0 0 0 5q�63    �� Bullet 5      �� 5      $��30��6%   $��30��60   $��0q�6<   .+�5    "   �� 
�������$��30��6@   $��30��6K   $��5��6W   5!   #   �� 5"   "   $��30��6(   $��30��63   ��5#   &   �� 5$   *    �� 
TargetType" ��60    �� Triggers 5%   .   �� 5&   2    �� Binding 6>   �� *Status+	 �� Value	 Negative6Y   .a�5'   :   �� 
 �� Property
 � �� �� Value$  ./images/close-browser.png��6S   5(   3   �� 5)   2    6>   �� *Status+	 	Fulfilled6Y   .a�5*   6   �� 
 � ��$ ./images/check2.png��6O   5+   3   5-   +   5/   '   50      ._�52      �� $��10 0 0 0q�6$   $��20(�6M   ��66   �� *Title+�� 53      $��10 0 0 0q�6(   $��20(�6T   ��6:   �� *
Progress+6d   55      56      57      59      5@      5A      
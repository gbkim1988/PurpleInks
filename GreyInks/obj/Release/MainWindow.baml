   M S B A M L   `   `   `  ���� C  ?GreyInks, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:GreyInksGreyInks      GreyInks.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:GreyInks       � WindowStyle$
  None=� � WindowStartupLocation$ CenterScreen=�$%��./images/#a허상고딕B 보통)�
������� � Title$ 
MainWindow��$	��800��$	��650�� � 	Resources    GreyInks.GreyConverters   EnumToStringConverter�� %   &	          ����  �� ImageSource$ images/blackboard.jpg��.���� $��0 0 0 0q�$
��Auto��$
��Auto��.+��� $��	0 20 0 10q�$0��*시스템보안파트 체크리스트 V0.1��$��40(�
��������� $��0 0 0 30q�$��20(�$-��' @ 안전한 PC 환경 검토 서비스���� -   $��DiagnosticCheckList��$��10 10 10 10q����� .R��� $
��Auto��$��0 0 0 5q� �� Bullet �� $��30��$��30��$��0q�.+��� 
�������$��30��$��30��$��5���� $��30��$��30������  �� 
TargetType" �� �� Triggers ��  �� Binding �� *Status+	 �� Value	 Negative.a��� 
 �� Property
 � �� �� Value$  ./images/close-browser.png����  �� *Status+	 	Fulfilled.a��� 
 � ��$ ./images/check2.png��._��� $��10 0 0 0q�$��20(����� *Title+�� $��10 0 0 0q�$��20(����� *
Progress+
   M S B A M L   `   `   `  ����  WpfApp1    FractalApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Fractals��5   	   $	��600��6   $	��800��6'   .��5      �� .@�5   
   �� $��
Mandelbrot��6   .��5      � $��10q�6    � RowDefinitions 5	      �� 5
      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      �� 5      $
��Auto �6$   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0��66   .+�5      �� -   5         ��Name$ btnMandelbrotUi��5      $��Start in UI��66   $��5q�6g   �� 5      -   5        $ btnMandelbrotBg��5      $��Start in Background��66   $��5q�6o   �� 5      -   5        $ btnCancelMandelbrot��5      $��Cancel��6:   $��5q�6j   ${�False��6u   5      � 5      -   5         �Name$ progressMandelbrot��5      $��1��6:   $��20��6G   $��5q�6S   �� 5      -   5         ��Name$ imgMandelbrot��5      $��2��6/   $��Uniform=�6<   5      5      �� 5   
   $��Julia��6   .��5      � $��10q�6    5      �� 5       $
��Auto �6$   �� 5!      $
��Auto �6$   �� 5"      �� 5#      $
��Auto �6$   .+�5&      �� $ 
Horizontal=�6   $��0��66   .+�5'      �� -   5        $ 
btnJuliaUi��5'      $��Start in UI��61   $��5q�6]   �� 5(      -   5        $ 
btnJuliaBg��5(      $��Start in Background��61   $��5q�6e   �� 5)      -   5        $ btnCancelJulia��5)      $��Cancel��65   $��5q�6`   ${�False��6k   �� 5*      $	��C: ��6    $��Center=�6+   �� 5+      -	   5         ��Name$ txtReal��5+      $��50��6/   $��5q�6:   $
��-0.4��6E   �� 5,      -
   5        $ txtImag��5,      $��50��6/   $��5q�6:   $	��0.6��6E   5-      � 5/      -   5        $ progressJulia��5/      $��1��65   $��20��6B   $��5q�6N   �� 51      -   5        $ imgJulia��51      $��2��6*   $��Uniform=�67   52      53      54      55      
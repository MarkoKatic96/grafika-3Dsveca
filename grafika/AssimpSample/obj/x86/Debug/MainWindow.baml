   M S B A M L   `   `   `  ����R  NSharpGL.WPF, Version=2.3.0.1, Culture=neutral, PublicKeyToken=27fc851303210b27>.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPFSharpGL.WPF  5   	    AssimpSample   AssimpSample.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   BsharpGL.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPF     5   	   -   5          � Title$  SharpGL WPF Application��5   	   $	��350��6)   $	��525��66   .��5      � $��0,29,0,0q�6    � RowDefinitions 5   
   �� 5      $	��18* �6   �� 5	      $	��12* �6   �� 5
      $
��263* �6    � ColumnDefinitions 5   
   �� 5      $	��11* �6   �� 5      $	��64* �6   �� 5      $
��177* �6   �� 5      $	��38* �6   �� 5      $
��227* �6   .+�5   
   �� $��	Rotacija:��6   $��0,-23,18,11q�6$   $��2��69   �� 5   
   -   5        $��46,-23,163,11q�5      $��2��6K   $��1��6_   6q   �� 5   
   $��Svetlosni izvor:��6   $��27,-22,61,10q�6+   $��2��6A   �� 5   
   -   5        $��ColorKutija��5      $��116,-23,208,11q�6&   $��3��6b   $��2��6v   .@�5      �� $��Narandzasta��6    �� 
IsSelected . 61   �� 5      $��Crvena��6   �� 5      $��Zelena��6   �� 5      $��Plava��6   5      �� 5   
   $��Faktor skaliranja:��6   $��24,-22,103,4q�6-   $��4��6C   �� 5    
   -   5        $��124,-22,5,11q�5       $��4��6L   6^      SharpGL.WPF.OpenGLControl  5!   
   -   5        $��openGLControl��5"         DrawFPS .    RenderContextType$	 FBO=�5$         	FrameRate$ 10Q�64   $��5��6C   $��1��6W   $��2��6d   5&      5'      
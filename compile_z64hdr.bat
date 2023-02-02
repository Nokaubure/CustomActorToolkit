@echo off

set "tmpdir=%cd%\tmp\z64hdr"

echo %tmpdir%

rmdir /S /Q "%tmpdir%"
mkdir "%tmpdir%"

set "z64hdr=%cd%\z64hdr"

set "PATH=%cd%\mips32-elf-toolchain\bin;%PATH%"

mips32-elf-gcc -I "%z64hdr%" -I "%z64hdr%\include" -I "%z64hdr%\include\include" -I "%z64hdr%\include\assets" -I "%z64hdr%\include\src" -G 0 -O1 -fno-reorder-blocks -std=gnu99 -mtune=vr4300 -march=vr4300 -mabi=32 -c -mips3 -mno-explicit-relocs -mno-memcpy -mno-check-zero-division -o "%tmpdir%\actor.o" "%input%"

IF %ERRORLEVEL% NEQ 0 (
  echo -
  echo /!\ Compilation failed!
  exit %ERRORLEVEL%
)

fado\fado -n actor -o "%tmpdir%\relocs.s" "%tmpdir%\actor.o"

mips32-elf-as -march=vr4300 -32 -Iinclude "%tmpdir%\relocs.s" -o "%tmpdir%\relocs.o"

mips32-elf-ld -L "%z64hdr%\common" -L "%z64hdr%\oot_mq_debug" --defsym=VRAM_START=%vram_start% -T "%z64hdr%\oot_mq_debug\syms.ld" -T ".\fado\script.ld" --emit-relocs -o "%tmpdir%\actor.zovl.o" "%tmpdir%\actor.o" "%tmpdir%\relocs.o"

IF %ERRORLEVEL% NEQ 0 (
  echo -
  echo /!\ Linking failed!
  exit %ERRORLEVEL%
)

mips32-elf-objcopy -j .zovl -O binary "%tmpdir%\actor.zovl.o" "%output%"

mips32-elf-objdump -t "%tmpdir%\actor.zovl.o" | findstr /I ".zovl" | findstr /I "constructor destructor init update main dest draw init_vars initvars" >> "%output_symbols%"

mips32-elf-objdump -t "%tmpdir%\actor.zovl.o" | findstr /I "_actorSegmentBssSize" >> "%output_symbols%"

echo done, output %output% !

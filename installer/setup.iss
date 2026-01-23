[Setup]
AppName=RuralTech
AppVersion=1.0.0
DefaultDirName={autopf}\RuralTech
DefaultGroupName=RuralTech
OutputDir=dist
OutputBaseFilename=RuralTech-Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "Crear icono en el escritorio"; GroupDescription: "Iconos adicionales:"
Name: "quicklaunchicon"; Description: "Crear icono en la barra de tareas"; GroupDescription: "Iconos adicionales:"; Flags: unchecked

[Files]
Source: "..\electron-app\dist\win-unpacked\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\electron-app\assets\*"; DestDir: "{app}\assets"; Flags: ignoreversion

[Icons]
Name: "{group}\RuralTech"; Filename: "{app}\RuralTech.exe"
Name: "{group}\Desinstalar RuralTech"; Filename: "{uninstallexe}"
Name: "{autodesktop}\RuralTech"; Filename: "{app}\RuralTech.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\RuralTech"; Filename: "{app}\RuralTech.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\RuralTech.exe"; Description: "Ejecutar RuralTech"; Flags: nowait postinstall skipifsilent

[Code]
procedure InitializeWizard();
begin
  // Crear carpeta en el escritorio
  CreateDir(ExpandConstant('{userdesktop}\RuralTech'));
  CreateDir(ExpandConstant('{userdesktop}\RuralTech\Documentos'));
  CreateDir(ExpandConstant('{userdesktop}\RuralTech\Reportes'));
  CreateDir(ExpandConstant('{userdesktop}\RuralTech\Backups'));
end;

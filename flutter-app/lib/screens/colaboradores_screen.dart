import 'package:flutter/material.dart';
import '../services/colaboradores_service.dart';
import '../services/upp_service.dart';

class ColaboradoresScreen extends StatefulWidget {
  final String? uppId;
  
  const ColaboradoresScreen({super.key, this.uppId});

  @override
  State<ColaboradoresScreen> createState() => _ColaboradoresScreenState();
}

class _ColaboradoresScreenState extends State<ColaboradoresScreen> {
  List<dynamic> _colaboradores = [];
  List<dynamic> _upps = [];
  String? _selectedUPPId;
  bool _isLoading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _selectedUPPId = widget.uppId;
    _loadData();
  }

  Future<void> _loadData() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      // Cargar UPPs
      _upps = await UPPService.getUPPs();
      
      // Si hay UPP seleccionada, cargar colaboradores
      if (_selectedUPPId != null) {
        _colaboradores = await ColaboradoresService.getColaboradoresByUPP(_selectedUPPId!);
      } else if (_upps.isNotEmpty) {
        _selectedUPPId = _upps[0]['id'].toString();
        _colaboradores = await ColaboradoresService.getColaboradoresByUPP(_selectedUPPId!);
      }
    } catch (e) {
      setState(() {
        _error = e.toString().replaceAll('Exception: ', '');
      });
    } finally {
      setState(() {
        _isLoading = false;
      });
    }
  }

  Future<void> _showAddColaboradorDialog() async {
    final nombreController = TextEditingController();
    final pinController = TextEditingController();
    final telefonoController = TextEditingController();
    String selectedRol = 'OPERARIO';

    await showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Agregar Colaborador', style: TextStyle(fontWeight: FontWeight.bold)),
        content: SingleChildScrollView(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              DropdownButtonFormField<String>(
                value: _selectedUPPId,
                decoration: const InputDecoration(
                  labelText: 'UPP *',
                  border: OutlineInputBorder(),
                ),
                items: _upps.map((upp) {
                  return DropdownMenuItem(
                    value: upp['id'].toString(),
                    child: Text(upp['nombrePredio'] ?? 'Sin nombre'),
                  );
                }).toList(),
                onChanged: (value) {
                  setState(() {
                    _selectedUPPId = value;
                  });
                },
              ),
              const SizedBox(height: 16),
              TextField(
                controller: nombreController,
                decoration: const InputDecoration(
                  labelText: 'Nombre Alias *',
                  border: OutlineInputBorder(),
                ),
              ),
              const SizedBox(height: 16),
              TextField(
                controller: pinController,
                decoration: const InputDecoration(
                  labelText: 'PIN (4-6 dígitos) *',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.number,
                obscureText: true,
              ),
              const SizedBox(height: 16),
              TextField(
                controller: telefonoController,
                decoration: const InputDecoration(
                  labelText: 'Teléfono (Opcional)',
                  border: OutlineInputBorder(),
                ),
                keyboardType: TextInputType.phone,
              ),
              const SizedBox(height: 16),
              DropdownButtonFormField<String>(
                value: selectedRol,
                decoration: const InputDecoration(
                  labelText: 'Rol *',
                  border: OutlineInputBorder(),
                ),
                items: const [
                  DropdownMenuItem(value: 'ENCARGADO', child: Text('Encargado')),
                  DropdownMenuItem(value: 'OPERARIO', child: Text('Operario')),
                  DropdownMenuItem(value: 'VETERINARIO', child: Text('Veterinario')),
                ],
                onChanged: (value) {
                  selectedRol = value!;
                },
              ),
            ],
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cancelar'),
          ),
          ElevatedButton(
            onPressed: () async {
              if (nombreController.text.isEmpty || pinController.text.isEmpty || _selectedUPPId == null) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('Completa todos los campos requeridos')),
                );
                return;
              }

              try {
                await ColaboradoresService.createColaborador({
                  'uppId': _selectedUPPId,
                  'nombreAlias': nombreController.text,
                  'pin': pinController.text,
                  'telefonoContacto': telefonoController.text.isEmpty ? null : telefonoController.text,
                  'rol': selectedRol,
                });
                Navigator.pop(context);
                _loadData();
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('Colaborador agregado exitosamente')),
                );
              } catch (e) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(e.toString().replaceAll('Exception: ', ''))),
                );
              }
            },
            style: ElevatedButton.styleFrom(backgroundColor: Colors.black),
            child: const Text('Agregar', style: TextStyle(color: Colors.white)),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Colaboradores', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.black)),
        backgroundColor: Colors.white,
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.black),
      ),
      backgroundColor: Colors.white,
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _error != null
              ? Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(_error!, style: const TextStyle(color: Colors.red)),
                      const SizedBox(height: 16),
                      ElevatedButton(
                        onPressed: _loadData,
                        style: ElevatedButton.styleFrom(backgroundColor: Colors.black),
                        child: const Text('Reintentar', style: TextStyle(color: Colors.white)),
                      ),
                    ],
                  ),
                )
              : Column(
                  children: [
                    // Selector de UPP
                    if (_upps.length > 1)
                      Container(
                        padding: const EdgeInsets.all(16),
                        color: Colors.grey.shade100,
                        child: DropdownButtonFormField<String>(
                          value: _selectedUPPId,
                          decoration: const InputDecoration(
                            labelText: 'Seleccionar UPP',
                            border: OutlineInputBorder(),
                            filled: true,
                            fillColor: Colors.white,
                          ),
                          items: _upps.map((upp) {
                            return DropdownMenuItem(
                              value: upp['id'].toString(),
                              child: Text(upp['nombrePredio'] ?? 'Sin nombre'),
                            );
                          }).toList(),
                          onChanged: (value) async {
                            setState(() {
                              _selectedUPPId = value;
                              _isLoading = true;
                            });
                            try {
                              _colaboradores = await ColaboradoresService.getColaboradoresByUPP(value!);
                            } catch (e) {
                              setState(() {
                                _error = e.toString();
                              });
                            } finally {
                              setState(() {
                                _isLoading = false;
                              });
                            }
                          },
                        ),
                      ),
                    // Lista de colaboradores
                    Expanded(
                      child: _colaboradores.isEmpty
                          ? Center(
                              child: Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: [
                                  const Icon(Icons.people_outline, size: 64, color: Colors.grey),
                                  const SizedBox(height: 16),
                                  const Text('No hay colaboradores registrados'),
                                  const SizedBox(height: 8),
                                  Text(
                                    _selectedUPPId == null ? 'Selecciona una UPP primero' : 'Agrega tu primer colaborador',
                                    style: const TextStyle(color: Colors.grey),
                                  ),
                                ],
                              ),
                            )
                          : ListView.builder(
                              padding: const EdgeInsets.all(16),
                              itemCount: _colaboradores.length,
                              itemBuilder: (context, index) {
                                final colaborador = _colaboradores[index];
                                return Card(
                                  margin: const EdgeInsets.only(bottom: 12),
                                  elevation: 2,
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(12),
                                    side: const BorderSide(color: Colors.black, width: 1),
                                  ),
                                  child: ListTile(
                                    leading: CircleAvatar(
                                      backgroundColor: Colors.black,
                                      child: Text(
                                        colaborador['nombreAlias'][0].toUpperCase(),
                                        style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold),
                                      ),
                                    ),
                                    title: Text(
                                      colaborador['nombreAlias'],
                                      style: const TextStyle(fontWeight: FontWeight.bold),
                                    ),
                                    subtitle: Column(
                                      crossAxisAlignment: CrossAxisAlignment.start,
                                      children: [
                                        Text('Rol: ${colaborador['rol']}'),
                                        if (colaborador['telefonoContacto'] != null)
                                          Text('Tel: ${colaborador['telefonoContacto']}'),
                                        Text(
                                          'Estatus: ${colaborador['estatus']}',
                                          style: TextStyle(
                                            color: colaborador['estatus'] == 'ACTIVO' ? Colors.green : Colors.red,
                                            fontWeight: FontWeight.bold,
                                          ),
                                        ),
                                      ],
                                    ),
                                    trailing: PopupMenuButton(
                                      itemBuilder: (context) => [
                                        PopupMenuItem(
                                          child: const Text('Cambiar Estatus'),
                                          onTap: () async {
                                            await Future.delayed(const Duration(milliseconds: 100));
                                            final nuevoEstatus = colaborador['estatus'] == 'ACTIVO' ? 'SUSPENDIDO' : 'ACTIVO';
                                            try {
                                              await ColaboradoresService.updateEstatus(
                                                colaborador['id'].toString(),
                                                nuevoEstatus,
                                              );
                                              _loadData();
                                            } catch (e) {
                                              ScaffoldMessenger.of(context).showSnackBar(
                                                SnackBar(content: Text(e.toString())),
                                              );
                                            }
                                          },
                                        ),
                                        PopupMenuItem(
                                          child: const Text('Eliminar', style: TextStyle(color: Colors.red)),
                                          onTap: () async {
                                            await Future.delayed(const Duration(milliseconds: 100));
                                            final confirm = await showDialog<bool>(
                                              context: context,
                                              builder: (context) => AlertDialog(
                                                title: const Text('Confirmar eliminación'),
                                                content: const Text('¿Estás seguro de eliminar este colaborador?'),
                                                actions: [
                                                  TextButton(
                                                    onPressed: () => Navigator.pop(context, false),
                                                    child: const Text('Cancelar'),
                                                  ),
                                                  ElevatedButton(
                                                    onPressed: () => Navigator.pop(context, true),
                                                    style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
                                                    child: const Text('Eliminar', style: TextStyle(color: Colors.white)),
                                                  ),
                                                ],
                                              ),
                                            );
                                            if (confirm == true) {
                                              try {
                                                await ColaboradoresService.deleteColaborador(colaborador['id'].toString());
                                                _loadData();
                                                ScaffoldMessenger.of(context).showSnackBar(
                                                  const SnackBar(content: Text('Colaborador eliminado')),
                                                );
                                              } catch (e) {
                                                ScaffoldMessenger.of(context).showSnackBar(
                                                  SnackBar(content: Text(e.toString())),
                                                );
                                              }
                                            }
                                          },
                                        ),
                                      ],
                                    ),
                                  ),
                                );
                              },
                            ),
                    ),
                  ],
                ),
      floatingActionButton: _selectedUPPId != null
          ? FloatingActionButton(
              onPressed: _showAddColaboradorDialog,
              backgroundColor: Colors.black,
              child: const Icon(Icons.add, color: Colors.white),
            )
          : null,
    );
  }
}

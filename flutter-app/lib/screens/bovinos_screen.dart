import 'package:flutter/material.dart';
import '../services/bovinos_service.dart';

class BovinosScreen extends StatelessWidget {
  const BovinosScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Bovinos', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.black)),
        backgroundColor: Colors.white,
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.black),
      ),
      backgroundColor: Colors.white,
      body: const Center(
        child: Text('Pantalla de Bovinos - En desarrollo'),
      ),
    );
  }
}

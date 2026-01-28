import 'package:flutter/material.dart';
import '../services/upp_service.dart';

class UPPsScreen extends StatelessWidget {
  const UPPsScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('UPPs', style: TextStyle(fontWeight: FontWeight.bold, color: Colors.black)),
        backgroundColor: Colors.white,
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.black),
      ),
      backgroundColor: Colors.white,
      body: const Center(
        child: Text('Pantalla de UPPs - En desarrollo'),
      ),
    );
  }
}

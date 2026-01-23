import 'package:flutter/material.dart';
import '../services/api_service.dart';
import 'login_screen.dart';

class DashboardScreen extends StatefulWidget {
  const DashboardScreen({super.key});

  @override
  State<DashboardScreen> createState() => _DashboardScreenState();
}

class _DashboardScreenState extends State<DashboardScreen> {
  List<dynamic> _animals = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _loadAnimals();
  }

  Future<void> _loadAnimals() async {
    try {
      final animals = await ApiService.getAnimals();
      setState(() {
        _animals = animals;
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _isLoading = false;
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error: $e')),
        );
      }
    }
  }

  Future<void> _logout() async {
    await ApiService.clearToken();
    if (mounted) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('RuralTech'),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: _logout,
          ),
        ],
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _animals.isEmpty
              ? const Center(child: Text('No hay animales registrados'))
              : ListView.builder(
                  itemCount: _animals.length,
                  itemBuilder: (context, index) {
                    final animal = _animals[index];
                    return Card(
                      margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                      child: ListTile(
                        leading: const Icon(Icons.pets, size: 40),
                        title: Text(animal['name'] ?? 'Sin nombre'),
                        subtitle: Text('Raza: ${animal['breed'] ?? 'N/A'}'),
                        trailing: Text('${animal['currentWeight'] ?? 0} kg'),
                      ),
                    );
                  },
                ),
    );
  }
}

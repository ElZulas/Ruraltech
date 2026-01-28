import 'package:flutter/material.dart';
import '../services/api_service.dart';
import 'login_screen.dart';
import 'colaboradores_screen.dart';
import 'animals_screen.dart';
import 'upps_screen.dart';
import 'bovinos_screen.dart';

class DashboardScreen extends StatefulWidget {
  const DashboardScreen({super.key});

  @override
  State<DashboardScreen> createState() => _DashboardScreenState();
}

class _DashboardScreenState extends State<DashboardScreen> {
  int _selectedIndex = 0;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: IndexedStack(
        index: _selectedIndex,
        children: const [
          HomeTab(),
          UPPsScreen(),
          AnimalsScreen(),
          BovinosScreen(),
          ColaboradoresScreen(),
        ],
      ),
      bottomNavigationBar: Container(
        decoration: const BoxDecoration(
          border: Border(top: BorderSide(color: Colors.black, width: 2)),
        ),
        child: BottomNavigationBar(
          currentIndex: _selectedIndex,
          onTap: (index) {
            setState(() {
              _selectedIndex = index;
            });
          },
          type: BottomNavigationBarType.fixed,
          selectedItemColor: Colors.black,
          unselectedItemColor: Colors.grey,
          backgroundColor: Colors.white,
          selectedLabelStyle: const TextStyle(fontWeight: FontWeight.bold),
          items: const [
            BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Inicio'),
            BottomNavigationBarItem(icon: Icon(Icons.business), label: 'UPPs'),
            BottomNavigationBarItem(icon: Icon(Icons.pets), label: 'Animales'),
            BottomNavigationBarItem(icon: Icon(Icons.agriculture), label: 'Bovinos'),
            BottomNavigationBarItem(icon: Icon(Icons.people), label: 'Colaboradores'),
          ],
        ),
      ),
    );
  }
}

class HomeTab extends StatelessWidget {
  const HomeTab({super.key});

  Future<void> _logout(BuildContext context) async {
    await ApiService.clearToken();
    if (context.mounted) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        title: const Text(
          'Cownect',
          style: TextStyle(fontWeight: FontWeight.bold, color: Colors.black, fontSize: 24),
        ),
        backgroundColor: Colors.white,
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.black),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout, color: Colors.black),
            onPressed: () => _logout(context),
            tooltip: 'Cerrar sesión',
          ),
        ],
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Header
            Container(
              padding: const EdgeInsets.all(24),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(20),
                border: Border.all(color: Colors.black, width: 2),
                boxShadow: [
                  BoxShadow(
                    color: Colors.black.withOpacity(0.1),
                    blurRadius: 10,
                    offset: const Offset(0, 5),
                  ),
                ],
              ),
              child: Column(
                children: [
                  Container(
                    width: 100,
                    height: 100,
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(25),
                      border: Border.all(color: Colors.black, width: 2),
                    ),
                    child: const Icon(Icons.pets, size: 50, color: Colors.black),
                  ),
                  const SizedBox(height: 16),
                  const Text(
                    'Bienvenido',
                    style: TextStyle(
                      fontSize: 28,
                      fontWeight: FontWeight.bold,
                      color: Colors.black,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Container(
                    width: 60,
                    height: 3,
                    color: Colors.black,
                  ),
                  const SizedBox(height: 8),
                  const Text(
                    'Sistema de Gestión Ganadera',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.black87,
                      fontWeight: FontWeight.w600,
                    ),
                    textAlign: TextAlign.center,
                  ),
                ],
              ),
            ),
            const SizedBox(height: 24),
            
            // Funcionalidades
            const Text(
              'Funcionalidades',
              style: TextStyle(
                fontSize: 22,
                fontWeight: FontWeight.bold,
                color: Colors.black,
              ),
            ),
            const SizedBox(height: 16),
            
            GridView.count(
              crossAxisCount: 2,
              shrinkWrap: true,
              physics: const NeverScrollableScrollPhysics(),
              crossAxisSpacing: 16,
              mainAxisSpacing: 16,
              childAspectRatio: 1.1,
              children: [
                _buildFeatureCard(
                  context,
                  icon: Icons.business,
                  title: 'UPPs',
                  subtitle: 'Unidades de Producción',
                  onTap: () {
                    // Navegar a UPPs - se implementará navegación
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Usa el menú inferior para navegar')),
                    );
                  },
                ),
                _buildFeatureCard(
                  context,
                  icon: Icons.pets,
                  title: 'Animales',
                  subtitle: 'Gestión de ganado',
                  onTap: () {
                    // Navegar a Animales - se implementará navegación
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Usa el menú inferior para navegar')),
                    );
                  },
                ),
                _buildFeatureCard(
                  context,
                  icon: Icons.agriculture,
                  title: 'Bovinos',
                  subtitle: 'Registro bovino',
                  onTap: () {
                    // Navegar a Bovinos - se implementará navegación
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Usa el menú inferior para navegar')),
                    );
                  },
                ),
                _buildFeatureCard(
                  context,
                  icon: Icons.people,
                  title: 'Colaboradores',
                  subtitle: 'Gestionar equipo',
                  onTap: () {
                    // Navegar a Colaboradores - se implementará navegación
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Usa el menú inferior para navegar')),
                    );
                  },
                ),
                _buildFeatureCard(
                  context,
                  icon: Icons.warehouse,
                  title: 'Infraestructura',
                  subtitle: 'Próximamente',
                  onTap: () {
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Funcionalidad en desarrollo')),
                    );
                  },
                ),
                _buildFeatureCard(
                  context,
                  icon: Icons.shopping_bag,
                  title: 'Marketplace',
                  subtitle: 'Próximamente',
                  onTap: () {
                    ScaffoldMessenger.of(context).showSnackBar(
                      const SnackBar(content: Text('Funcionalidad en desarrollo')),
                    );
                  },
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildFeatureCard(
    BuildContext context, {
    required IconData icon,
    required String title,
    required String subtitle,
    required VoidCallback onTap,
  }) {
    return InkWell(
      onTap: onTap,
      child: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(16),
          border: Border.all(color: Colors.black, width: 2),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.1),
              blurRadius: 8,
              offset: const Offset(0, 4),
            ),
          ],
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Container(
              padding: const EdgeInsets.all(16),
              decoration: BoxDecoration(
                color: Colors.black,
                borderRadius: BorderRadius.circular(12),
              ),
              child: Icon(icon, color: Colors.white, size: 32),
            ),
            const SizedBox(height: 12),
            Text(
              title,
              style: const TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
                color: Colors.black,
              ),
            ),
            const SizedBox(height: 4),
            Text(
              subtitle,
              style: const TextStyle(
                fontSize: 12,
                color: Colors.grey,
              ),
              textAlign: TextAlign.center,
            ),
          ],
        ),
      ),
    );
  }
}

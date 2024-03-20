# ToDo

## Entregas

### PEC 1: Informe Preliminar

- [ ] Elaborar lista de tareas

## Basics

### Rework Cardhouse

- [ ] Pilas de cartas compactas (que no tienen todas las cartas instanciadas siempre)
  - [X] Funcionalidad básica
  - [ ] Expandir al hacer click
  - [ ] Detalles estéticos
    - [ ] Poner un sprite que simule el resto de cartas del mazo
    - [ ] Poner el número de cartas del mazo

#### Mecánicas

- [ ] Crear un stack (para que puedan resolverse los spells)
  - [ ] Funcionalidad básica
    - [x] Spiral Layout
    - [ ] Efectos al poner la carta
    - [ ] Guardar quién ha puesto la carta en el stack
  - [ ] Sistema de targets
  - [ ] Resaltar cartas targeteadas dentro de un mazo
  - [ ] Opciones al castear
  - [ ] Permitir targets a otros spells

- [ ] Cartas "Proxy" (permiten a una carta estar en dos sitios a la vez)
  - [ ] Castear desde la pila de descarte
  
- [ ] Mejorar las animaciones
  - [x] Hacer otra curva de velocidad para los seekers
  - [ ] Controlar el movimiento de cartas con corrutinas
    - [x] Corrutinas en el TransferOperator
  
- [ ] Hacer focus en las cartas
  - [x] Focus parcial al hacer hoverx
  - [ ] Mostrar tooltips y descripciones correspondientes


- [ ] Card Stats
  - [ ] Cost
  - [ ] Modes
  - [ ] Card Ownership
  - [ ] Card controller

#### Modo Exploración

- [ ] Game loop básico
  - [x] Poner las habitaciónes y su contenido
  - [ ] Escoger la habitación siguiente
  - [ ] Revelar el contenido escogido
  - [ ] Efectos asociados
    - [ ] A habitaciones
    - [ ] A contenidos
# ToDo

## Entregas

### DOCUMENTACIÓN

- [ ] Diagrama de Gant es demasiado generico
- [ ] Presupuesto - 
Texto + tabla (salarios empleados) + Costes variables??
- [ ] Revisar Objetivos
  - [ ] Objetivos Secundarios como extras
- [ ] Usar calculos del presupuesto en estrategia de marketing y modelo de negocio
  - [ ] Calcular unidades necesarias para rentabilizar el juego
- [ ] 4.1
  - [ ] Tipos de assets que necesitamos
  - [ ] Scripts importantes


## Basics

### Rework Cardhouse

- [ ] Pilas de cartas compactas (que no tienen todas las cartas instanciadas siempre)
  - [X] Funcionalidad básica
  - [x] Expandir al hacer click
  - [ ] Que las cartas targeteadas se salgan un poco del mazo
  - [ ] Detalles estéticos
    - [ ] Poner un sprite que simule el resto de cartas del mazo
    - [x] Poner el número de cartas del mazo

- [ ] Subgrupos
  - [x] Contenido habitaciones
  - [ ] Habilidades
  - [ ] Encantamientos
  - [ ] Falso subgrupo (contiene cartas que en realidad están en otro grupo)
  - [ ] Enviar cartas a otro grupo cuando este se destruye
    - [x] Grupo por defecto
    - [x] Que dependa de cada carta

#### Mecánicas

- [x] Crear un stack (para que puedan resolverse los spells)
  - [ ] Funcionalidad básica
    - [x] Spiral Layout
    - [x] Efectos al poner la carta
    - [ ] Guardar quién ha puesto la carta en el stack
  - [ ] Sistema de targets
  - [ ] Opciones al castear
  - [ ] Permitir targets a otros spells

- [ ] Cartas "Proxy" (permiten a una carta estar en dos sitios a la vez)
  - [ ] Castear desde la pila de descarte
  
- [x] Mejorar las animaciones
  - [x] Hacer otra curva de velocidad para los seekers
  - [x] Controlar el movimiento de cartas con corrutinas
    - [x] Corrutinas en el TransferOperator
  
- [ ] Attached cards (Cartas que se ponen debajo de otras)
  - [x] Attach groups
  - [ ] Movement as a unit (Requiere rework del tema seekers)
  - [ ] Recursive shrinking
  
- [ ] Hacer focus en las cartas
  - [x] Focus parcial al hacer hoverx
  - [ ] Mostrar tooltips y descripciones correspondientes


- [ ] Card Stats
  - [ ] Card speeds
    - [ ] Action
    - [ ] Reaction
    - [ ] Ritual
  - [x] Cost
  - [ ] Abilities
    - [ ] Triggered
    - [ ] Activated
      - [ ] Activation speeds
  - [ ] Card Context
    - [x] Card Ownership
    - [x] Card controller
    - [ ] Card Zone (zona en la que se encuentra la carta o desde la que se ha lanzado)
    - [x] Values and targets
  - [ ] Modifiers
    - [ ] De duración temporal
    - [ ] De cast
    - [ ] De cambio de zona
      - [ ] Flashback

#### Modo Exploración

- [ ] Game loop básico
  - [x] Poner las habitaciónes y su contenido
  - [x] Escoger la habitación siguiente
  - [x] Revelar el contenido escogido
  - [ ] Triggers asociados
    - [ ] A habitaciones
    - [x] A contenidos
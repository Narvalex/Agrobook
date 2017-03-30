// lista de usuarios
fromStream('$ce-agrobook.usuarios')
.when({
   '$init': function (s, e) {
       return { listaDeUsuarios: [] };
   },
   'nuevoUsuarioCreado': function (s, e) {
       s.listaDeUsuarios.push(e.data.usuario);
   }
});
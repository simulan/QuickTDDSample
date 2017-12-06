using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMLCreatorLibrary.Models.Deserializers {
    public class UML : TypeAdapterChain {
        private Dictionary<Type, TypeAdapter<Object>> registeredSerializers = new Dictionary<Type, TypeAdapter<Object>>();

        public UML() {
            RegisterDefaultSerializers();
        }
        private void RegisterDefaultSerializers() {
            registeredSerializers.Add(typeof(Method), new MethodSerializer(this as TypeAdapterChain));
            registeredSerializers.Add(typeof(Argument), new ArgumentSerializer());
            registeredSerializers.Add(typeof(Class), new ClassSerializer(this as TypeAdapterChain));
        }
        public void AddSerializerTypeAdapter(Type typeOf,TypeAdapter<Object> typeAdapter) {
            registeredSerializers.Add(typeOf, typeAdapter);
        }

        public T Deserialize<T>(string input) where T : class {
            return registeredSerializers[typeof(T)].Deserialize(input) as T;
        }
        public override TypeAdapter<Object> provide<T>() {
            return registeredSerializers[typeof(T)];
        }
    }
}

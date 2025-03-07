﻿using System.Globalization;
using F23.StringSimilarity;
using F23.StringSimilarity.Interfaces;
using System.Text;
using chess4connect.Models.SocketComunication.MessageTypes;


namespace chess4connect.Services
{
    public class SmartSearchFriends
    {
        private const double THRESHOLD = 0.75;
        private readonly INormalizedStringSimilarity _stringSimilarityComparer;
        private readonly UserService _userService;

        public SmartSearchFriends(UserService userService) {
            _stringSimilarityComparer = new JaroWinkler();
            _userService = userService;
        }

        public List<FriendModel> Search(string query, List<FriendModel> users)
        {
            List<FriendModel> result;

            // Si la consulta está vacía o solo tiene espacios en blanco, devolvemos todos los items
            if (string.IsNullOrWhiteSpace(query))
            {
                result = users;
            }
            // En caso contrario, realizamos la búsqueda
            else
            {
                // Limpiamos la query y la separamos por espacios
                string[] queryKeys = GetKeys(ClearText(query));
                // Aquí guardaremos los items que coincidan
                List<FriendModel> matches = new List<FriendModel>();

                foreach (FriendModel item in users)
                {
                    // Limpiamos el item y lo separamos por espacios
                    string[] itemKeys = GetKeys(ClearText(item.UserName));

                    // Si coincide alguna de las palabras de item con las de query
                    // entonces añadimos item a la lista de coincidencias
                    if (IsMatch(queryKeys, itemKeys))
                    {
                        matches.Add(item);
                    }
                }

                result = matches;
            }
            return result;

        }

        private bool IsMatch(string[] queryKeys, string[] itemKeys)
        {
            bool isMatch = false;

            for (int i = 0; !isMatch && i < itemKeys.Length; i++)
            {
                string itemKey = itemKeys[i];

                for (int j = 0; !isMatch && j < queryKeys.Length; j++)
                {
                    string queryKey = queryKeys[j];

                    isMatch = IsMatch(itemKey, queryKey);
                }
            }

            return isMatch;
        }

        // Hay coincidencia si las palabras son las mismas o si item contiene query o si son similares
        private bool IsMatch(string itemKey, string queryKey)
        {
            return itemKey == queryKey
                || itemKey.Contains(queryKey)
                || _stringSimilarityComparer.Similarity(itemKey, queryKey) >= THRESHOLD;
        }

        // Separa las palabras quitando los espacios
        private string[] GetKeys(string query)
        {
            return query.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        // Normaliza el texto quitándole las tildes y pasándolo a minúsculas
        private string ClearText(string text)
        {
            return RemoveDiacritics(text.ToLower());
        }

        // Quita las tildes a un texto
        private string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder(normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        internal List<FriendModel> Search(string query)
        {
            throw new NotImplementedException();
        }
    }
}


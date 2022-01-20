using System;
using Godot;
using BibliotecaViva.DTO;

namespace BibliotecaViva.BLL.Utils
{
    public static class ImportadorDeBinariosUtil
    {
        public static Texture BuscarImagem(string nomeImagem, string formato, string caminho)
        {
            var imagem = new Image();
            var texturaDaImagem = new ImageTexture();
            var caminhoComFormato = caminho + nomeImagem + "." + formato;
            var caminhoImport = caminhoComFormato + ".import";

            imagem.Load(caminhoComFormato);
            texturaDaImagem.CreateFromImage(imagem);
            LimparArquivosTemporarios(caminhoComFormato, caminhoImport);
            
            return texturaDaImagem;
        }
        public static AudioStream BuscarAudio(string nomeImagem, string formato, string base64)
        {
            var data = Convert.FromBase64String(base64);
            
            switch(formato)
            {
                case (".wav"):
                    return new AudioStreamSample()
                    {
                        Data = data
                    }; 
                case (".mp3"):
                    return new AudioStreamMP3()
                    {
                        Data = data
                    };
                case (".ogg"):
                    return new AudioStreamOGGVorbis()
                    {
                        Data = data
                    };
                default:
                    throw new Exception("Formato de áudio não suportado, por favor use .wav, .mp3 ou .ogg");
            }
        }
        public static Texture GerarImagem(string nomeImagem, string formato, string base64)
        {
            var caminho = CarregarBinario(nomeImagem, formato, base64);
            return BuscarImagem(nomeImagem, formato, caminho);
        }
        public static AudioStream GerarAudio(string nomeAudio, string formato, string base64)
        {
            var caminho = CarregarBinario(nomeAudio, formato, base64);
            return BuscarAudio(nomeAudio, formato, caminho);
        }
        public static void LimparArquivosTemporariosDeAudio(string nomeImagem, string formato)
        {
            var caminho = "./TEMP/";
            var caminhoComFormato = caminho + nomeImagem + "." + formato;
            var caminhoImport = caminhoComFormato + ".import";
            LimparArquivosTemporarios(caminhoComFormato, caminhoImport);
        }
        private static AudioStream ObterStream(string formato, string caminhoDoArquivo)
        {
            switch(formato)
            {
                case (".wav"):
                    return new AudioStreamSample();
                case (".mp3"):
                    return new AudioStreamMP3();
                case (".ogg"):
                    return new AudioStreamOGGVorbis();
                default:
                    throw new Exception("Formato de áudio não suportado, por favor use .wav, .mp3 ou .ogg");
            }
        }

        private static string CarregarBinario(string nomeBinario, string formato, string base64)
        {
            if(!System.IO.Directory.Exists("TEMP"))
                System.IO.Directory.CreateDirectory("TEMP");
            var caminho = "./TEMP/";
            System.IO.File.WriteAllBytes(caminho + nomeBinario + "." + formato, Convert.FromBase64String(base64));
            return caminho;
        }
        private static void LimparArquivosTemporarios(string caminhoComFormato, string caminhoImport)
        {
            if (System.IO.File.Exists(caminhoComFormato))
                System.IO.File.Delete(caminhoComFormato);
            if (System.IO.File.Exists(caminhoImport))
                System.IO.File.Delete(caminhoImport);
        }
        public static string ObterBase64(string nomeImagem, string formato, string caminho)
        {
            return ObterBase64(caminho + nomeImagem, formato);
        }
        public static string ObterBase64(string caminho, string formato)
        {
            return ObterBase64(caminho + formato);
        }
        public static string ObterBase64(string caminho)
        {
            return System.Convert.ToBase64String(System.IO.File.ReadAllBytes(caminho));
        }
    }
}
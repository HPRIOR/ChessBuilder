﻿using Models.State.Board;
using UnityEngine;
using View.Interfaces;
using View.Prefab.Interfaces;
using View.Utils;
using Zenject;

namespace View.Prefab.Spawners
{
    public class PieceSpawner : MonoBehaviour, IPieceSpawner
    {
        private void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (RenderInfo.SpriteAssetPath != "")
                spriteRenderer.sprite = Resources.Load<Sprite>(RenderInfo.SpriteAssetPath);

            // set parent object and piece position
            GameObject pieceGameObject;
            (pieceGameObject = gameObject).transform.parent = GameObject.FindGameObjectWithTag("Pieces")?.transform;

            // change position of game object 
            Vector3 position = Position.GetVector();
            position += new Vector3(0, 0, -1); // move 'forward' so that there are no box collider clashes
            pieceGameObject.transform.position = position;
        }

        public Position Position { get; private set; }
        public IPieceRenderInfo RenderInfo { get; private set; }

        [Inject]
        public void Construct(IPieceRenderInfo pieceRenderInfo, Position position)
        {
            RenderInfo = pieceRenderInfo;
            Position = position;
        }


        public override string ToString() => $"{RenderInfo}\n{Position}\n";

        public class Factory : PlaceholderFactory<IPieceRenderInfo, Position, PieceSpawner>
        {
        }
    }
}
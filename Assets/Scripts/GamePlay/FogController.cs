using UnityEngine;

namespace GamePlay
{
    public class FogController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private ParticleSystem fogParticles;
        private const float FogRadiusInvisible = 4f;
        private ParticleSystem.Particle[] _particles;

        private void Start()
        {
            _particles = new ParticleSystem.Particle[fogParticles.main.maxParticles];
        }

        private void Update()
        {
            var playerPosition = Player2DVector(player.position);
            var numParticles = fogParticles.GetParticles(_particles);
            for (int i = 0; i < numParticles; i++)
            {
                var distanceToPlayer = Vector2.Distance(_particles[i].position, playerPosition);
                if (distanceToPlayer <= FogRadiusInvisible)
                {
                    _particles[i].startSize = 0f;
                }
                else
                {
                    _particles[i].startSize = distanceToPlayer - FogRadiusInvisible;
                }
            }
            fogParticles.SetParticles(_particles, numParticles);
        }

        public Vector2 Player2DVector(Vector3 playerPosition)
        {
            return new Vector2(playerPosition.x, -playerPosition.z);
        }
    }
}
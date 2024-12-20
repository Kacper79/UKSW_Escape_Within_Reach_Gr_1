using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject enemy_main_go;

    private GameObject player_go;

    private void Awake()
    {
        player_go = FindObjectOfType<PlayerInputController>().gameObject;
    }

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        float angle = Vector3.Angle(enemy_main_go.transform.forward, player_go.transform.position - enemy_main_go.transform.position);

        angle = Vector3.Angle(enemy_main_go.transform.right, player_go.transform.position - enemy_main_go.transform.position) <= 90.0f ? angle : -angle;

        transform.localRotation = Quaternion.Euler(0.0f, 
                                                   180.0f + angle, 
                                                   0.0f);
    }

    public void ChangeBarValue(int max_value, int current_value)
    {
        bar.transform.localScale = new(CalculateXScale(max_value, current_value), bar.transform.localScale.y, bar.transform.localScale.z);
    }

    private float CalculateXScale(int max_value, int current_value)
    {
        return ((float)current_value / (float)max_value) * (-1);
    }
}

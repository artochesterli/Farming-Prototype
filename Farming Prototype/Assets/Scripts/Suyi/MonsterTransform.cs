using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterTransform
{
    public abstract void OnUse();
    public virtual void OnUnUse()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().OnTransformBack();
    }
}

public class BuffaloTransform : MonsterTransform
{
    public BuffaloData BuffaloData;
    public GameObject Player;
    public BuffaloTransform(BuffaloData _bd)
    {
        BuffaloData = _bd;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public BuffaloTransform(BuffaloTransform _bf)
    {
        BuffaloData = _bf.BuffaloData;
        Player = _bf.Player;
    }

    public override void OnUse()
    {
        Player.GetComponent<PlayerController>().OnTransform<MonsterBuffalo>(BuffaloData);
        Player.GetComponent<FormManager>().CurrentForm = Form.Bull;
    }
}


public class FlowerTransform : MonsterTransform
{
    public FlowerMonsterData FlowerData;
    public GameObject Player;

    public FlowerTransform(FlowerMonsterData _fd)
    {
        FlowerData = _fd;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public FlowerTransform(FlowerTransform _bf)
    {
        FlowerData = _bf.FlowerData;
        Player = _bf.Player;
    }

    public override void OnUse()
    {
        Player.GetComponent<PlayerController>().OnTransform<MonsterFlower>(FlowerData);
        Player.GetComponent<FormManager>().CurrentForm = Form.Flower;
    }
}

public class SlimeTransform : MonsterTransform
{
    public SlimeData SlimeData;
    public GameObject Player;
    public SlimeTransform(SlimeData _bd)
    {
        SlimeData = _bd;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public SlimeTransform(SlimeTransform _bf)
    {
        SlimeData = _bf.SlimeData;
        Player = _bf.Player;
    }

    public override void OnUse()
    {
        Player.GetComponent<PlayerController>().OnTransform<SlimeAI>(SlimeData);
        Player.GetComponent<FormManager>().CurrentForm = Form.Slime;
    }
}

public class DryadTransform : MonsterTransform
{
    public MonsterDryadData DryadData;
    public GameObject Player;
    public DryadTransform(MonsterDryadData _bd)
    {
        DryadData = _bd;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public DryadTransform(DryadTransform _bf)
    {
        DryadData = _bf.DryadData;
        Player = _bf.Player;
    }

    public override void OnUse()
    {
        Player.GetComponent<PlayerController>().OnTransform<DryadAI>(DryadData);
        Player.GetComponent<FormManager>().CurrentForm = Form.Dryad;

    }
}
using UnityEngine;
using System;
using System.Linq;

public class CloudHandler : MonoBehaviour
{
  public float Speed;

  // Start is called before the first frame update
  void Start()
  {
		var width = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;

		var cloudNums = new[] { 0, 1 };/*Enumerable
			.Range(0, _context.GO(ContextObjects.Background).transform.childCount)
			.ToArray();*/
		Array
			.ForEach(cloudNums, (num) => {
          var position = transform.GetChild(num).transform.position;
					transform.GetChild(num).transform.position = new Vector3((num) * width, position.y, position.z);
        });
  }

  // Update is called once per frame
  void Update()
  {
		var width = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;

		transform.position = new Vector3(Speed * Time.deltaTime, transform.position.y, transform.position.z);

		var backgroundNums = Enumerable
			.Range(0, transform.childCount)
			.ToArray();
		Array
			.ForEach(backgroundNums, (num) => {
					var child = transform.GetChild(num);
					child.transform.position -= new Vector3(Speed * Time.deltaTime, 0f, 0f);
					if(child.transform.position.x < -(width*1))
            child.transform.position += new Vector3(width * transform.childCount, 0f, 0f);
					});
  }
}

import fs from "node:fs";
import path from "node:path";
import { generateApi } from "swagger-typescript-api";

const OUTPUT_PATH = path.resolve(process.cwd(), "./src/api");
const TEMPLATES_PATH = path.join(OUTPUT_PATH, "templates");
const GATEWAY_URL = "http://localhost:5098";

async function main() {
  try {
    // Генерация API из Swagger
    const apiOutput = await generateApi({
      url: `${GATEWAY_URL}/swagger/v1/swagger.json`,
      output: OUTPUT_PATH,
      cleanOutput: false, 
      templates: TEMPLATES_PATH,
      extractEnums: true,
      sortTypes: true,
      sortRoutes: true,
      modular: true,
      extraTemplates: [
        { name: "index", path: OUTPUT_PATH + "/templates/index.ejs" },
      ],
      unwrapResponseData: true, 
      moduleNameFirstTag: true,
    });

    // Очистка и подготовка директории
    fs.rmSync(OUTPUT_PATH + "/api", { force: true, recursive: true });

    const files = fs.readdirSync(OUTPUT_PATH);
    for (const file of files) {
      if (
        file.endsWith(".ts") ||
        (file !== "templates" &&
          file !== "build-api.tsx" &&
          file !== "index.tsx" )
      ) {
        fs.unlinkSync(path.join(OUTPUT_PATH, file));
      }
    }

    fs.mkdirSync(OUTPUT_PATH + "/api");

    // Сохранение сгенерированных файлов API
    apiOutput.files.forEach((fileInfo) => {
      if (fileInfo.fileName == "index") {
        fs.writeFileSync(
          OUTPUT_PATH + "/" + fileInfo.fileName + ".tsx",
          fileInfo.fileContent
        );
      } else {
        let fileName = fileInfo.fileName;

        if (fileName != "data-contracts" && fileName != "http-client")
          fileName += "Api.tsx";
        else fileName += ".tsx";

        fs.writeFileSync(
          OUTPUT_PATH + "/api/" + fileName,
          fileInfo.fileContent
        );
      }
    });

    console.log("Генерация API клиентов завершена успешно");
  } catch (e) {
    console.error("Ошибка во время генерации:", e);
    process.exit(1);
  }
}

main();
